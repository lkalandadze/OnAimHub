﻿using Hub.Application.Models.Coin;
using Hub.Application.Services.Abstract;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Entities.Coins;
using Hub.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;
using System.Linq;

namespace Hub.Application.Features.PromotionFeatures.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand, PromotionResponse>
{
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IBackgroundJobScheduler _jobScheduler;
    private readonly IJobRepository _jobRepository;
    private readonly ICoinService _coinService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IServiceRepository _serviceRepository;

    public CreatePromotionCommandHandler(
        ISegmentRepository segmentRepository,
        IPromotionRepository promotionRepository,
        IBackgroundJobScheduler jobScheduler,
        IJobRepository jobRepository,
        ICoinService coinService,
        IUnitOfWork unitOfWork,
        IServiceRepository serviceRepository)
    {
        _segmentRepository = segmentRepository;
        _promotionRepository = promotionRepository;
        _jobScheduler = jobScheduler;
        _jobRepository = jobRepository;
        _coinService = coinService;
        _unitOfWork = unitOfWork;
        _serviceRepository = serviceRepository;
    }

    public async Task<PromotionResponse> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        //var rootCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(request);
        //var rootFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(request).ToList();
        //var rootStatus = CheckmateValidations.Checkmate.IsValid(request);

        //var treeStatus = CheckmateValidations.Checkmate.IsValid(request, true);

        //var treeFailedCheckers = CheckmateValidations.Checkmate.GetFailedChecks(request, true).ToList();
        //var treeCheckContainer = CheckmateValidations.Checkmate.GetCheckContainersWithInstance(request, "", true);

        //if (!CheckmateValidations.Checkmate.IsValid(request, true))
        //{
        //    throw new CheckmateException(CheckmateValidations.Checkmate.GetFailedChecks(request, true));
        //}
        try
        {
            if (request.EndDate <= request.StartDate)
            {
                throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, $"EndDate must be later than StartDate.");
            }

            var segments = await _segmentRepository.QueryAsync(s => request.SegmentIds.Any(sId => sId == s.Id));

            if (segments.Count != request.SegmentIds.Count())
            {
                throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "One or more Segment IDs are invalid.");
            }

            //var services = await _serviceRepository
            //        .Query()
            //        .Where(s => request.ServiceIds.Contains(s.Id))
            //        .ToListAsync(cancellationToken);

            //                if (services.Count != request.ServiceIds.Count())
            //                {
            //                    throw new ApiException(ApiExceptionCodeTypes.BusinessRuleViolation, "One or more Service IDs are invalid.");
            //                }

            var promotion = new Promotion(
                request.StartDate,
                request.EndDate,
                request.Title,
                request.Description,
                request.CorrelationId,
                createdByUserId: request.CreatedByUserId,
                templateId: request.TemplateId,
                segments: segments
            );

            //foreach (var service in services)
            //{
            //    promotion.Services.Add(service);
            //}


            await _promotionRepository.InsertAsync(promotion);
            await _unitOfWork.SaveAsync();

            var mappedCoins = request.Coins.Select(coin => CreateCoinModel.ConvertToEntity(coin, promotion.Id))
                                           .ToList();

            if (request.Coins.FirstOrDefault(c => c.CoinType == CoinType.Out) is CreateOutCoinModel outCoinModel)
            {
                var withdrawOptions = await _coinService.GetWithdrawOptions(outCoinModel);
                var withdrawOptionGroups = await _coinService.GetWithdrawOptionGroups(outCoinModel);

                var outCoin = mappedCoins.OfType<OutCoin>()
                                         .FirstOrDefault(c => c.CoinType == CoinType.Out);

                if (outCoin != null)
                {
                    if (withdrawOptions.Any())
                        outCoin.AddWithdrawOptions(withdrawOptions);

                    if (withdrawOptionGroups.Any())
                        outCoin.AddWithdrawOptionGroups(withdrawOptionGroups);
                }
            }

            promotion.SetCoins(mappedCoins);

            _promotionRepository.Update(promotion);
            await _unitOfWork.SaveAsync();

            SchedulePromotionStatusJobs(promotion);

            var res = new PromotionResponse
            {
                PromotionId = promotion.Id,
                Coins = mappedCoins.Select(x => new PromotionResponseCoin
                {
                    Id = x.Id,
                    CoinName = x.Name,
                }).ToList(),
            };

            return res;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    private void SchedulePromotionStatusJobs(Promotion promotion)
    {
        // Create jobs for status transitions
        var upcomingJob = new Job(
            name: $"Promotion-{promotion.Id}-Upcoming",
            description: $"Set promotion {promotion.Title} to Upcoming",
            isActive: true,
            jobType: JobType.Custom,
            jobCategory: JobCategory.PromotionStatusUpdate,
            executionTime: (promotion.StartDate - TimeSpan.FromDays(1)).TimeOfDay
        );

        var startJob = new Job(
            name: $"Promotion-{promotion.Id}-Start",
            description: $"Set promotion {promotion.Title} to Started",
            isActive: true,
            jobType: JobType.Custom,
            jobCategory: JobCategory.PromotionStatusUpdate,
            executionTime: promotion.StartDate.TimeOfDay
        );

        var finishJob = new Job(
            name: $"Promotion-{promotion.Id}-Finish",
            description: $"Set promotion {promotion.Title} to Finished",
            isActive: true,
            jobType: JobType.Custom,
            jobCategory: JobCategory.PromotionStatusUpdate,
            executionTime: promotion.EndDate.TimeOfDay
        );

        // Save jobs in the repository
        _jobRepository.InsertAsync(upcomingJob);
        _jobRepository.InsertAsync(startJob);
        _jobRepository.InsertAsync(finishJob);
        _unitOfWork.SaveAsync().GetAwaiter().GetResult(); // Ensure jobs are saved synchronously before scheduling

        // Schedule the jobs using Hangfire
        _jobScheduler.ScheduleJob(upcomingJob, GenerateCronExpression(promotion.StartDate - TimeSpan.FromDays(1)));
        _jobScheduler.ScheduleJob(startJob, GenerateCronExpression(promotion.StartDate));
        _jobScheduler.ScheduleJob(finishJob, GenerateCronExpression(promotion.EndDate));
    }

    private string GenerateCronExpression(DateTimeOffset date)
    {
        return $"{date.Second} {date.Minute} {date.Hour} {date.Day} {date.Month} ?";
    }
}
public class PromotionResponse
{
    public int PromotionId { get; set; }
    public List<PromotionResponseCoin> Coins { get; set; }
}
public class PromotionResponseCoin
{
    public string Id { get; set; }
    public string CoinName { get; set; }
}