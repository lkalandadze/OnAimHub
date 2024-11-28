using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnAim.Admin.APP.Features.SegmentFeatures.Queries.GetById.BlackListedPlayers;
using OnAim.Admin.APP.Services.Abstract;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Segment;
using System.Net.Http.Headers;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.APP.Services.Admin.AuthServices.Auth;
using OnAim.Admin.APP.Services.Hub.ClientServices;

namespace OnAim.Admin.APP.Services.Hub.Segment;

public class SegmentService : ISegmentService
{
    private readonly IHubApiClient _hubApiClient;
    private readonly IReadOnlyRepository<Domain.HubEntities.Segment> _segmentRepository;
    private readonly IReadOnlyRepository<PlayerSegmentAct> _playerSegmentActRepository;
    private readonly IReadOnlyRepository<PlayerSegmentActHistory> _playerSegmentActHistoryRepository;
    private readonly ISecurityContextAccessor _securityContextAccessor;
    private readonly HubApiClientOptions _options;

    public SegmentService(
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        IReadOnlyRepository<Domain.HubEntities.Segment> segmentRepository,
        IReadOnlyRepository<PlayerSegmentAct> _playerSegmentActRepository,
        IReadOnlyRepository<PlayerSegmentActHistory> _playerSegmentActHistoryRepository,
        ISecurityContextAccessor securityContextAccessor
        )
    {
        _hubApiClient = hubApiClient;
        _segmentRepository = segmentRepository;
        this._playerSegmentActRepository = _playerSegmentActRepository;
        this._playerSegmentActHistoryRepository = _playerSegmentActHistoryRepository;
        _securityContextAccessor = securityContextAccessor;
        _options = options.Value;
    }

    public async Task<ApplicationResult> AssignSegmentToPlayers(IEnumerable<string> segmentIds, IFormFile file)
    {
        using var multipartContent = new MultipartFormDataContent();

        if (file != null)
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            multipartContent.Add(fileContent, "file", file.FileName);
        }

        multipartContent.Add(new StringContent(string.Join(",", segmentIds)), "SegmentIds");
        multipartContent.Add(new StringContent(_securityContextAccessor.UserId.ToString()), "ByUserId");

        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Admin/AssignSegmentsToPlayers", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to assign players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(),
        };
    }

    public async Task<ApplicationResult> AssignSegmentToPlayer(string segmentId, int playerId)
    {
        var req = new
        {
            PlayerId = playerId,
            SegmentId = segmentId,
            ByUserId = _securityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJsonAndSerializeResultTo<object>(
            $"{_options.Endpoint}Admin/AssignSegmentToPlayer?segmentId={req.SegmentId}&playerId={req.PlayerId}",
            req
            );

        if (result != null)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to assign segment");
    }

    public async Task<ApplicationResult> BlockSegmentForPlayers(IEnumerable<string> segmentIds, IFormFile file)
    {
        using var multipartContent = new MultipartFormDataContent();

        if (file != null)
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            multipartContent.Add(fileContent, "file", file.FileName);
        }

        multipartContent.Add(new StringContent(string.Join(",", segmentIds)), "SegmentIds");
        multipartContent.Add(new StringContent(_securityContextAccessor.UserId.ToString()), "ByUserId");

        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Admin/BlockSegmentsForPlayers", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to Block Players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(),
        };
    }

    public async Task<ApplicationResult> BlockSegmentForPlayer(string segmentId, int playerId)
    {
        var req = new
        {
            PlayerId = playerId,
            SegmentId = segmentId,
            ByUserId = _securityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/BlockSegmentForPlayer?segmentId={req.SegmentId}&playerId={req.PlayerId}", req);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to block segment for player");
    }

    public async Task<ApplicationResult> CreateSegment(string id, string description, int priorityLevel)
    {
        var req = new
        {
            Id = id,
            Description = description,
            PriorityLevel = priorityLevel,
            CreatedByUserId = _securityContextAccessor.UserId,
        };

        var result = await _hubApiClient.PostAsJsonAndSerializeResultTo<object>($"{_options.Endpoint}Admin/CreateSegment", req);

        if (result != null)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to add segment");
    }

    public async Task<ApplicationResult> DeleteSegment(string id)
    {
        var result = await _hubApiClient.Delete($"{_options.Endpoint}Admin/DeleteSegment?id={id}");

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to delete segment");
    }

    public async Task<ApplicationResult> UnAssignPlayersToSegment(IEnumerable<string> segmentIds, IFormFile file)
    {
        using var multipartContent = new MultipartFormDataContent();

        if (file != null)
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            multipartContent.Add(fileContent, "file", file.FileName);
        }

        multipartContent.Add(new StringContent(string.Join(",", segmentIds)), "SegmentIds");
        multipartContent.Add(new StringContent(_securityContextAccessor.UserId.ToString()), "ByUserId");

        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Admin/UnassignSegmentsToPlayers", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to unassign players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(),
        };
    }

    public async Task<ApplicationResult> UnAssignSegmentForPlayer(string segmentId, int playerId)
    {
        var req = new
        {
            PlayerId = playerId,
            SegmentId = segmentId,
            ByUserId = _securityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/UnassignSegmentToPlayer?segmentId={req.SegmentId}&playerId={req.PlayerId}", req);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to unassign segment");
    }

    public async Task<ApplicationResult> UnBlockSegmentForPlayer(string segmentId, int playerId)
    {
        var req = new
        {
            PlayerId = playerId,
            SegmentId = segmentId,
            ByUserId = _securityContextAccessor.UserId
        };

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/UnblockSegmentForPlayer?segmentId={req.SegmentId}&playerId={req.PlayerId}", req);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to unblock segment for player");
    }

    public async Task<ApplicationResult> UnBlockSegmentForPlayers(IEnumerable<string> segmentIds, IFormFile file)
    {
        using var multipartContent = new MultipartFormDataContent();

        if (file != null)
        {
            var fileContent = new StreamContent(file.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            multipartContent.Add(fileContent, "file", file.FileName);
        }

        multipartContent.Add(new StringContent(string.Join(",", segmentIds)), "SegmentIds");
        multipartContent.Add(new StringContent(_securityContextAccessor.UserId.ToString()), "ByUserId");

        var response = await _hubApiClient.PostMultipartAsync($"{_options.Endpoint}Admin/UnblockSegmentsForPlayers", multipartContent);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HubAPIRequestFailedException($"Failed to Unblock Players to segment. Status Code: {response.StatusCode}. Response: {errorContent}");
        }

        return new ApplicationResult
        {
            Success = true,
            Data = await response.Content.ReadAsStringAsync(),
        };

    }

    public async Task<ApplicationResult> UpdateSegment(string id, string description, int priorityLevel)
    {
        var request = new
        {
            Id = id,
            Description = description,
            PriorityLevel = priorityLevel
        };
        var result = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/UpdateSegment", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult { Success = true };
        }

        throw new Exception("Failed to update segment");
    }

    public async Task<ApplicationResult> GetAll(int? pageNumber, int? pageSize)
    {
        var segments = _segmentRepository.Query().AsNoTracking();

        var totalCount = await segments.CountAsync();

        var pageNumberr = pageNumber ?? 1;
        var pageSizee = pageSize ?? 25;

        var res = segments
       .Select(x => new SegmentListDto
       {
           Id = x.Id,
           Name = x.Id,
           Description = x.Description,
           Priority = x.PriorityLevel,
           IsDeleted = x.IsDeleted,
           CreatedBy = x.CreatedByUserId,
           TotalPlayers = x.Players.Count(),
           LastUpdate = null,
       })
       .Skip((pageNumberr - 1) * pageSizee)
       .Take(pageSizee);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<SegmentListDto>
            {
                PageNumber = pageNumberr,
                PageSize = pageSizee,
                //TotalCount = totalCount,
                Items = await res.ToListAsync()
            },
        };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var segment = await _segmentRepository
            .Query(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Description,
                x.PriorityLevel
            })
            .FirstOrDefaultAsync();

        if (segment == null)
            throw new NotFoundException("Segment not found.");

        var res = new SegmentDto
        {
            SegmentId = segment.Id,
            SegmentName = segment.Id,
            PriorityLevel = segment.PriorityLevel,
        };

        return new ApplicationResult { Success = true, Data = res };
    }

    public async Task<ApplicationResult> GetActivePlayers(string segmentId, FilterBy filter)
    {
        if (segmentId == null)
            throw new BadRequestException("Segment Not Found");

        var blacklistedPlayerIds = await _segmentRepository.Query()
               .Where(segment => segment.Id == segmentId)
               .SelectMany(segment => segment.BlockedPlayers.Select(player => player.Id))
               .ToListAsync();

        var activePlayersQuery = _segmentRepository.Query()
                .Where(segment => segment.Id == segmentId)
                .SelectMany(segment => segment.Players)
                .Where(player => !blacklistedPlayerIds.Contains(player.Id));

        if (!string.IsNullOrEmpty(filter.UserName))
        {
            activePlayersQuery = activePlayersQuery.Where(player => player.UserName.Contains(filter.UserName));
        }

        var totalCount = await activePlayersQuery.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var pagedPlayers = await activePlayersQuery
            .OrderBy(player => player.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(player => new SegmentPlayerDto
            {
                PlayerId = player.Id,
                PlayerName = player.UserName
            })
            .Skip(pageNumber)
            .Take(pageSize)
            .ToListAsync();

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<SegmentPlayerDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = pagedPlayers
            }
        };
    }

    public async Task<ApplicationResult> GetBlackListedPlayers(string segmentId, FilterBy filter)
    {
        if (string.IsNullOrEmpty(segmentId))
            throw new BadRequestException("Segment ID is required.");

        var blacklistedPlayersQuery = _segmentRepository.Query()
            .Where(segment => segment.Id == segmentId)
            .SelectMany(segment => segment.BlockedPlayers);

        if (!string.IsNullOrEmpty(filter.UserName))
        {
            blacklistedPlayersQuery = blacklistedPlayersQuery
                .Where(player => player.UserName.Contains(filter.UserName));
        }

        var totalCount = await blacklistedPlayersQuery.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var pagedList = await blacklistedPlayersQuery
            .OrderBy(player => player.UserName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(player => new SegmentPlayerDto
            {
                PlayerId = player.Id,
                PlayerName = player.UserName,
                ReasonNote = null
            })
            .ToListAsync();

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<SegmentPlayerDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = pagedList
            }
        };
    }

    public async Task<ApplicationResult> GetActs(string segmentId)
    {
        var playerSegmentActs = await _playerSegmentActRepository.Query(x => x.SegmentId == segmentId).Include(x => x.Action).ToListAsync();

        var res = playerSegmentActs.Select(x => new ActsDto
        {
            Id = x.Id,
            UploadedBy = x.ByUserId,
            Quantity = x.TotalPlayers,
            Type = x.Action?.Name,
        });

        return new ApplicationResult
        {
            Success = true,
            Data = res
        };
    }

    public async Task<ApplicationResult> GetActsHistory(int playerSegmentActId)
    {
        var history = await _playerSegmentActHistoryRepository
           .Query(x => x.PlayerSegmentActId == playerSegmentActId)
           .Include(x => x.Player)
           .Include(x => x.PlayerSegmentAct)
           .ToListAsync();

        var res = history.Select(x => new ActsHistoryDto
        {
            Id = x.Id,
            Note = null,
            PlayerName = x.Player.UserName,
            PlayerId = x.PlayerId,
            Quantity = 1,
            UploadedBy = x.PlayerSegmentAct?.ByUserId,
            UploadedOn = null,
            Type = x.PlayerSegmentAct?.Action?.Name,
        });

        return new ApplicationResult
        {
            Success = true,
            Data = res
        };
    }

    public async Task<ApplicationResult> GetGeneralSegmentActs(SegmentActsFilter filter)
    {
        var query = _playerSegmentActRepository.Query();

        if (filter.SegmentId != null)
            query = query.Where(x => x.SegmentId == filter.SegmentId);

        if (filter.UserId != null)
            query = query.Where(x => x.ByUserId == filter.UserId);

        //if (request.Filter.DateFrom.HasValue)
        //    query = query.Where(x => x.DateCreated >= request.Filter.DateFrom.Value);

        //if (request.Filter.DateTo.HasValue)
        //    query = query.Where(x => x.DateCreated <= request.Filter.DateTo.Value);

        var totalCount = await query.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        bool sortDescending = filter.SortDescending.GetValueOrDefault();

        if (filter.SortBy == "id" || filter.SortBy == "Id")
        {
            query = sortDescending
                ? query.OrderByDescending(x => x.Id)
                : query.OrderBy(x => x.Id);
        }
        //else if (request.Filter.SortBy == "playerName" || request.Filter.SortBy == "PlayerName")
        //{
        //    query = sortDescending
        //        ? query.OrderByDescending(x => x.UserName)
        //        : query.OrderBy(x => x.UserName);
        //}

        var res = query.Select(x => new ActsDto
        {
            Id = x.Id,
            Note = null,
            UploadedBy = x.ByUserId,
            Quantity = x.TotalPlayers,
            Type = x.Action.Name,
            UploadedOn = null
        })
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize);

        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<ActsDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync()
            },
        };
    }

    public async Task<ApplicationResult> GetGeneralSegmentActsHistory(SegmentActsFilter filter)
    {
        var query = _playerSegmentActHistoryRepository.Query();

        var totalCount = await query.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        if (filter.SegmentId != null)
        {
            query = query.Where(x => x.Player.Segments.Any(ur => filter.SegmentId.Contains(ur.Id)));
        }

        if (filter.UserId.HasValue && filter.UserId.Value != 0)
        {
            query = query.Where(x => x.PlayerId == filter.UserId.Value);
        }

        var res = await query
            .Select(x => new ActsHistoryDto
            {
                Id = x.Id,
                Note = null,
                PlayerId = x.PlayerId,
                PlayerName = x.Player.UserName ?? "Unknown",
                UploadedBy = null,
                UploadedOn = null,
                Quantity = x.PlayerSegmentAct.TotalPlayers,
                Type = x.PlayerSegmentAct.Action.Name ?? null
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new ApplicationResult
        {
            Success = true,
            Data = new PaginatedResult<ActsHistoryDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = res,
            },
        };
    }
}