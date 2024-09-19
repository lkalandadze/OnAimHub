﻿using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Role;
using OnAim.Admin.Shared.DTOs.User;
using OnAim.Admin.Shared.Paging;

namespace OnAim.Admin.APP.Queries.User.GetAllUser
{
    public sealed class GetAllUserQueryHandler : IQueryHandler<GetAllUserQuery, ApplicationResult>
    {
        private readonly IRepository<Infrasturcture.Entities.User> _repository;

        public GetAllUserQueryHandler(IRepository<Infrasturcture.Entities.User> repository)
        {
            _repository = repository;
        }
        public async Task<ApplicationResult> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.Query(x =>
                            string.IsNullOrEmpty(request.UserFilter.Name) || x.FirstName.Contains(request.UserFilter.Name));

            if (request.UserFilter.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.UserFilter.IsActive.Value);
            }

            if (request.UserFilter.IsDeleted.HasValue)
            {
                query = query.Where(x => x.IsDeleted == request.UserFilter.IsDeleted);
            }

            if (!request.UserFilter.IsActive.HasValue && !request.UserFilter.IsDeleted.HasValue)
            {
                
            }

            if (request.UserFilter.RoleIds != null && request.UserFilter.RoleIds.Any())
            {
                query = query.Where(x => x.UserRoles.Any(ur => request.UserFilter.RoleIds.Contains(ur.RoleId)));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var pageNumber = request.UserFilter.PageNumber ?? 1;
            var pageSize = request.UserFilter.PageSize ?? 25;

            bool sortDescending = request.UserFilter.SortDescending.GetValueOrDefault();

            if (request.UserFilter.SortBy == "Id" || request.UserFilter.SortBy == "id")
            {
                query = sortDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);
            }
            else if (request.UserFilter.SortBy == "Name" || request.UserFilter.SortBy == "name")
            {
                query = sortDescending ? query.OrderByDescending(x => x.FirstName) : query.OrderBy(x => x.FirstName);
            }
            else if (request.UserFilter.SortBy == "LastName" || request.UserFilter.SortBy == "lastName")
            {
                query = sortDescending ? query.OrderByDescending(x => x.LastName) : query.OrderBy(x => x.LastName);
            }

            var res = query
                .Select(x => new UsersModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted,
                    Phone = x.Phone,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    Roles = x.UserRoles.Select(xx => new RoleDto
                    {
                        Id = xx.RoleId,
                        Name = xx.Role.Name,
                        IsActive = xx.Role.IsActive,
                    }).ToList(),
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new ApplicationResult
            {
                Success = true,
                Data = new PaginatedResult<UsersModel>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    Items = await res.ToListAsync()
                },
            };
        }
    }
}
