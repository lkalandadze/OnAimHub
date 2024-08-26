﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Infrasturcture.Entities;
using OnAim.Admin.Infrasturcture.Extensions;
using OnAim.Admin.Infrasturcture.Models.Response;
using OnAim.Admin.Infrasturcture.Models.Response.Endpoint;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.EndPoint.GetAll
{
    public class GetAllEndpointQueryHandler : IRequestHandler<GetAllEndpointQuery, ApplicationResult>
    {
        private readonly IRepository<Endpoint> _repository;

        public GetAllEndpointQueryHandler(IRepository<Endpoint> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllEndpointQuery request, CancellationToken cancellationToken)
        {
            var query = _repository
                .Query(x =>
                         (string.IsNullOrEmpty(request.Filter.Name) || x.Name.Contains(request.Filter.Name)) &&
                         (!request.Filter.IsActive.HasValue || x.IsActive == request.Filter.IsActive.Value) &&
                         (!request.Filter.Type.HasValue || x.Type == request.Filter.Type.Value) &&
                         (!request.Filter.IsEnable.HasValue || x.IsEnabled == request.Filter.IsEnable.Value)
                     );

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.Filter.PageNumber ?? 1;
            var pageSize = request.Filter.PageSize ?? 25;

            bool sortDescending = request.Filter.SortDescending.GetValueOrDefault();

            if (request.Filter.SortBy == "Id")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id);
            }
            else if (request.Filter.SortBy == "Name")
            {
                query = sortDescending
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name);
            }

            var endpoints = query
                .Select(x => new EndpointResponseModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Type = ToHttpMethodExtension.ToHttpMethod(x.Type),
                IsActive = x.IsActive,
                IsEnabled = x.IsEnabled,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
            })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<EndpointResponseModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await endpoints.ToListAsync()
                }
            };
        }
    }
}
