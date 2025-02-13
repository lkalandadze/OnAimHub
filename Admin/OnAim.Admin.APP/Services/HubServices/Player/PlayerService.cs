﻿using OnAim.Admin.Contracts.Dtos.Refer;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Dtos.Transaction;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;
using OnAim.Admin.APP.Services.HubServices.Player;

namespace OnAim.Admin.APP.Services.Hub.Player;

public class PlayerService : IPlayerService
{
    private readonly IHubApiClient _hubApiClient;
    private readonly HubApiClientOptions _options;
    private readonly IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository;
    private readonly IReadOnlyRepository<PlayerBalance> _playerBalanceRepository;
    private readonly IReadOnlyRepository<PlayerBan> _playerBanRepository;
    private readonly IReadOnlyRepository<Transaction> _transactionRepository;
    private readonly IReadOnlyRepository<ReferralDistribution> _referalRepository;
    private readonly IReadOnlyRepository<PlayerBalance> _playerBalanaceRepository;
    private readonly IReadOnlyRepository<PlayerLog> _playerLogRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;
    private readonly IReadOnlyRepository<PlayerProgress> _playerProgressRepository;
    private readonly IReadOnlyRepository<Domain.HubEntities.Segment> _segmentRepo;
    private readonly ILeaderBoardApiClient _leaderBoardApiClient;
    private readonly LeaderBoardApiClientOptions _leaderBoardOptions;

    public PlayerService(
        IHubApiClient hubApiClient,
        IOptions<HubApiClientOptions> options,
        IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> playerRepository,
        IReadOnlyRepository<PlayerBalance> playerBalanceRepository,
        IReadOnlyRepository<PlayerBan> playerBanRepository,
        IReadOnlyRepository<Transaction> transactionRepository,
        IReadOnlyRepository<ReferralDistribution> referalRepository,
        IReadOnlyRepository<PlayerBalance> playerBalanaceRepository,
        IReadOnlyRepository<PlayerLog> playerLogRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository,
        IReadOnlyRepository<PlayerProgress> playerProgressRepository,
        IReadOnlyRepository<Domain.HubEntities.Segment> segmentRepo,
        ILeaderBoardApiClient leaderBoardApiClient,
        IOptions<LeaderBoardApiClientOptions> leaderBoardOptions
        )
    {
        _hubApiClient = hubApiClient;
        _options = options.Value;
        _playerRepository = playerRepository;
        _playerBalanceRepository = playerBalanceRepository;
        _playerBanRepository = playerBanRepository;
        _transactionRepository = transactionRepository;
        _referalRepository = referalRepository;
        _playerBalanaceRepository = playerBalanaceRepository;
        _playerLogRepository = playerLogRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _playerProgressRepository = playerProgressRepository;
        _segmentRepo = segmentRepo;
        _leaderBoardApiClient = leaderBoardApiClient;
        _leaderBoardOptions = leaderBoardOptions.Value;
    }

    public async Task<ApplicationResult<bool>> BanPlayer(int playerId, DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        var request = new
        {
            PlayerId = playerId,
            ExpireDate = expireDate,
            IsPermanent = isPermanent,
            Description = description
        };

        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/BanPlayer", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult<bool> { Success = true };
        }

        throw new BadRequestException("Failed to Ban player");
    }

    public async Task<ApplicationResult<bool>> RevokeBan(int id)
    {
        var request = new
        {
            Id = id
        };

        var result = await _hubApiClient.PutAsJson($"{_options.Endpoint}Admin/RevokePlayerBan", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult<bool> { Success = true };
        }

        throw new BadRequestException("Failed to revoke ban!");
    }

    public async Task<ApplicationResult<bool>> UpdateBan(int id, DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        var request = new
        {
            Id = id,
            ExpireDate = expireDate,
            IsPermanent = isPermanent,
            Description = description
        };
        var result = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/UpdateBannedPlayer", request);

        if (result.IsSuccessStatusCode)
        {
            return new ApplicationResult<bool> { Success = true };
        }

        throw new BadRequestException("Failed to update ban!");
    }

    public async Task<ApplicationResult<PaginatedResult<PlayerListDto>>> GetAll(PlayerFilter filter)
    {
        var sortableFields = new List<string> { "Id", "UserName" };

        var palyers = _playerRepository.Query(x =>
                        string.IsNullOrEmpty(filter.Name) || EF.Functions.Like(x.UserName, $"{filter.Name}%")).AsNoTracking();

        if (filter.IsBanned.HasValue)
            palyers = palyers.Where(x => x.IsBanned == filter.IsBanned.Value);

        if (filter.SegmentIds?.Any() == true)
            palyers = palyers.Where(x => x.Segments.Any(ur => filter.SegmentIds.Contains(ur.Id)));

        if (filter.DateFrom.HasValue)
            palyers = palyers.Where(x => x.LastVisitedOn >= filter.DateFrom.Value);

        if (filter.DateFrom.HasValue)
            palyers = palyers.Where(x => x.LastVisitedOn <= filter.DateTo.Value);

        var totalCount = await palyers.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;
        bool sortDescending = filter.SortDescending.GetValueOrDefault();

        if (filter.SortBy == "Id" || filter.SortBy == "id")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.Id)
                : palyers.OrderBy(x => x.Id);
        }
        else if (filter.SortBy == "userName" || filter.SortBy == "UserName")
        {
            palyers = sortDescending
                ? palyers.OrderByDescending(x => x.UserName)
                : palyers.OrderBy(x => x.UserName);
        }

        var res = palyers
            .Select(x => new PlayerListDto
            {
                Id = x.Id,
                UserName = x.UserName ?? null,
                RegistrationDate = null,
                LastVisit = null,
                Segment = x.Segments
                            .OrderByDescending(ps => ps.PriorityLevel)
                            .Select(ps => ps.Id)
                            .FirstOrDefault(),
                IsBanned = x.IsBanned,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);


        return new ApplicationResult<PaginatedResult<PlayerListDto>>
        {
            Success = true,
            Data = new PaginatedResult<PlayerListDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = sortableFields,
            },
        };
    }

    public async Task<ApplicationResult<PlayerDto>> GetById(int id)
    {
        var player = await _playerRepository
           .Query(x => x.Id == id)
           .Include(x => x.Segments)
           .FirstOrDefaultAsync();

        if (player == null)
            throw new NotFoundException("Player Not Found!");

        var referee = await _referalRepository.Query(x => x.ReferrerId == player.ReferrerId).FirstOrDefaultAsync();

        var referrals = await _referalRepository
            .Query(x => x.ReferrerId == player.Id)
            .Select(x => new ReferralDto
            {
                Id = x.Id,
                InvitedDateTime = x.DateCreated,
                UserName = x.Referral.UserName,
            })
            .ToListAsync();

        Domain.HubEntities.PlayerEntities.Player refPlayer = null;

        if (referee != null)
            refPlayer = await _playerRepository.Query(x => x.ReferrerId == referee.ReferrerId).FirstOrDefaultAsync();

        //var logs = await _playerLogRepository.Query(x => x.PlayerId == player.Id).Include(x => x.PlayerLogType).ToListAsync();

        var result = new PlayerDto
        {
            Id = player.Id,
            PlayerName = player.UserName,
            IsBanned = player.IsBanned,
            Segments = player.Segments.Select(x => new SegmentDto
            {
                Id = x.Id,
                Description = x.Description,
            }).ToList(),
            RegistrationDate = player.RegistredOn,
            LastVisit = player.LastVisitedOn,
            Referee = referee != null ? new RefereeDto
            {
                Id = referee.Id,
                UserName = refPlayer?.UserName,
                InvitedDateTime = referee.DateCreated
            } : null,
            //PlayerLogs = logs.Select(x => new PlayerLogDto
            //{
            //    Id = x.Id,
            //    Log = x.Log,
            //    TimeStamp = x.Timestamp,
            //    PlayerLogType = x.PlayerLogType?.Name,
            //}).ToList(),
            Referrals = referrals
        };

        return new ApplicationResult<PlayerDto>
        {
            Data = result,
            Success = true,
        };
    }

    public async Task<ApplicationResult<List<PlayerBalanceDto>>> GetBalance(int id)
    {
        var balances = _playerBalanceRepository.Query(x => x.PlayerId == id);

        var result = balances.Select(x => new PlayerBalanceDto
        {
            Id = x.Id,
            Amount = x.Amount,
            Currency = x.Coin.Name,
        });

        return new ApplicationResult<List<PlayerBalanceDto>>
        {
            Success = true,
            Data = await result.ToListAsync()
        };
    }

    public async Task<ApplicationResult<bool>> AddBalanceToPlayer(AddBalanceDto command)
    {
        try
        {
            var res = await _hubApiClient.PostAsJson($"{_options.Endpoint}Admin/AddBalanceToPlayer", command);
            return new ApplicationResult<bool> { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult<PlayerBan>> GetBannedPlayer(int id)
    {
        var palyer = await _playerBanRepository.Query(x => x.Id == id).FirstOrDefaultAsync();

        return new ApplicationResult<PlayerBan>
        {
            Success = true,
            Data = palyer ?? null,
        };
    }

    public async Task<ApplicationResult<List<BannedPlayerListDto>>> GetAllBannedPlayers()
    {
        var banned = _playerBanRepository.Query();

        var result = banned.Select(x => new BannedPlayerListDto
        {
            Id = x.Id,
            PlayerId = x.PlayerId,
            PlayerName = x.Player.UserName,
            Description = x.Description,
            DateBanned = x.DateBanned,
            ExpireDate = x.ExpireDate,
            IsPermanent = x.IsPermanent,
            IsRevoked = x.IsRevoked,
            RevokeDate = x.RevokeDate,
        });


        return new ApplicationResult<List<BannedPlayerListDto>>
        {
            Success = true,
            Data = await result.ToListAsync()
        };
    }

    public async Task<UserActiveLeaderboards> GetLeaderBoardResultByPlayer(int playerId)
    {
        var leaderboardResults = await _leaderBoardApiClient.Get<UserActiveLeaderboards>(
            $"{_leaderBoardOptions.Endpoint}LeaderboardProgress/GetUserActiveLeaderboards?PlayerId={playerId}");

        return leaderboardResults;
    }

    public async Task<ApplicationResult<PlayerProgressDto>> GetPlayerProgress(int id)
    {
        var progress = await _playerProgressRepository.Query(x => x.PlayerId == id).FirstOrDefaultAsync();

        var result = new PlayerProgressDto
        {
            DailyProgress = progress?.Progress ?? 0,
            TotalProgress = progress?.Progress ?? 0
        };

        return new ApplicationResult<PlayerProgressDto>
        {
            Success = true,
            Data = result
        };
    }

    public async Task<ApplicationResult<PaginatedResult<PlayerTransactionDto>>> GetPlayerTransaction(int id, BaseFilter filter)
    {
        var transaction = _transactionRepository.Query(x => x.PlayerId == id);

        var totalCount = await transaction.CountAsync();

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var res = transaction
            .Select(x => new PlayerTransactionDto
            {
                Id = x.Id,
                Game = null,
                Type = x.Type.Id,
                Amount = x.Amount,
                Coin = x.Coin.Name,
                //Date = null,
                Status = x.Status.Name,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PlayerTransactionDto>>
        {
            Data = new PaginatedResult<PlayerTransactionDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = new List<string>(),
            },
            Success = true
        };
    }

    public async Task<ApplicationResult<PaginatedResult<PlayerLogDto>>> GetPlayerLogs(int id, BaseFilter filter)
    {
        var logs = _playerLogRepository.Query(x => x.PlayerId == id).Include(x => x.PlayerLogType);

        var totalCount = await logs.CountAsync();
        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? 25;

        var res = logs
            .Select(x => new PlayerLogDto
            {
                Id = x.Id,
                Log = x.Log,
                Action = x.PlayerLogType.Name,
                Date = x.Timestamp,
            })
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new ApplicationResult<PaginatedResult<PlayerLogDto>>
        {
            Data = new PaginatedResult<PlayerLogDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = await res.ToListAsync(),
                SortableFields = new List<string>(),
            },
            Success = true
        };
    }
}