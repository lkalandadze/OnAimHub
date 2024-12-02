using Hub.Application.Models.Coin;
using Hub.Application.Services.Abstract.BackgroundJobs;
using Hub.Domain.Abstractions;
using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Domain.Enum;
using MediatR;
using Shared.Application.Exceptions;
using Shared.Application.Exceptions.Types;

namespace Hub.Application.Features.PromotionFeatures.Commands.Create;

public class CreatePromotionCommandHandler : IRequestHandler<CreatePromotionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISegmentRepository _segmentRepository;
    private readonly IPromotionRepository _promotionRepository;
    private readonly IBackgroundJobScheduler _jobScheduler;
    private readonly IJobRepository _jobRepository;

    public CreatePromotionCommandHandler(IUnitOfWork unitOfWork, ISegmentRepository segmentRepository, IPromotionRepository promotionRepository, IBackgroundJobScheduler jobScheduler, IJobRepository jobRepository)
    {
        _unitOfWork = unitOfWork;
        _segmentRepository = segmentRepository;
        _promotionRepository = promotionRepository;
        _jobScheduler = jobScheduler;
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
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

        //TODO: ქოინების ვალიდაციები
        if (request.Coins.Where(c => c.CoinType == CoinType.In).Count() != 1)
        {
        }

        var promotion = new Promotion(
            request.StartDate,
            request.EndDate,
            request.Title,
            request.Description,
            request.CorrelationId,
            segments: segments
        );

        await _promotionRepository.InsertAsync(promotion);
        await _unitOfWork.SaveAsync();

        SchedulePromotionStatusJobs(promotion);

        var mappedCoins = request.Coins.Select(coin => CreateCoinModel.ConvertToEntity(coin, promotion.Id)).ToList();
        promotion.SetCoins(mappedCoins);

        _promotionRepository.Update(promotion);

        try
        {
            await _unitOfWork.SaveAsync();
        }
        catch (Exception ex)
        {
            throw;
        }

        return Unit.Value;
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