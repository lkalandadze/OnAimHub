using Microsoft.EntityFrameworkCore;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.LeaderBoradEntities;
using OnAim.Admin.Contracts.ApplicationInfrastructure;
using OnAim.Admin.Contracts.Dtos.Player;
using OnAim.Admin.Contracts.Dtos.Refer;
using OnAim.Admin.Contracts.Dtos.Segment;
using OnAim.Admin.Contracts.Dtos.Transaction;
using OnAim.Admin.Contracts.Paging;
using OnAim.Admin.CrossCuttingConcerns.Exceptions;
using OnAim.Admin.APP.Services.Hub.ClientServices;
using OnAim.Admin.Domain.HubEntities.PlayerEntities;
using OnAim.Admin.APP.Services.HubServices.Player;
using OnAim.Admin.Infrasturcture.Interfaces;

namespace OnAim.Admin.APP.Services.Hub.Player;

public class PlayerService : IPlayerService
{
    private readonly IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> _playerRepository;
    private readonly IReadOnlyRepository<PlayerBalance> _playerBalanceRepository;
    private readonly IReadOnlyRepository<PlayerBan> _playerBanRepository;
    private readonly IReadOnlyRepository<Transaction> _transactionRepository;
    private readonly IReadOnlyRepository<ReferralDistribution> _referalRepository;
    private readonly IReadOnlyRepository<PlayerBalance> _playerBalanaceRepository;
    private readonly IReadOnlyRepository<PlayerLog> _playerLogRepository;
    private readonly ILeaderBoardReadOnlyRepository<LeaderboardResult> _leaderboardResultRepository;
    private readonly IReadOnlyRepository<PlayerProgress> _playerProgressRepository;
    private readonly HubApiClientOptions _options;
    private readonly HubClientService _hubClientService;

    public PlayerService(
        IReadOnlyRepository<Domain.HubEntities.PlayerEntities.Player> playerRepository,
        IReadOnlyRepository<PlayerBalance> playerBalanceRepository,
        IReadOnlyRepository<PlayerBan> playerBanRepository,
        IReadOnlyRepository<Transaction> transactionRepository,
        IReadOnlyRepository<ReferralDistribution> referalRepository,
        IReadOnlyRepository<PlayerBalance> playerBalanaceRepository,
        IReadOnlyRepository<PlayerLog> playerLogRepository,
        ILeaderBoardReadOnlyRepository<LeaderboardResult> leaderboardResultRepository,
        IReadOnlyRepository<PlayerProgress> playerProgressRepository,
        HubClientService hubClientService
        )
    {
        _playerRepository = playerRepository;
        _playerBalanceRepository = playerBalanceRepository;
        _playerBanRepository = playerBanRepository;
        _transactionRepository = transactionRepository;
        _referalRepository = referalRepository;
        _playerBalanaceRepository = playerBalanaceRepository;
        _playerLogRepository = playerLogRepository;
        _leaderboardResultRepository = leaderboardResultRepository;
        _playerProgressRepository = playerProgressRepository;
        _hubClientService = hubClientService;
    }

    public async Task<ApplicationResult> BanPlayer(int playerId, DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        var request = new CreatePlayerBanCommand
        {
            PlayerId = playerId,
            ExpireDate = expireDate,
            Description = description,
            IsPermanent = isPermanent
        };

        try
        {
            await _hubClientService.BanPlayerAsync(request);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> RevokeBan(int id)
    {
        var request = new RevokePlayerBanCommand
        {
            Id = id
        };

        try
        {
            await _hubClientService.RevokePlayerBanAsync(request);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> UpdateBan(int id, DateTimeOffset? expireDate, bool isPermanent, string description)
    {
        var request = new UpdatePlayerBanCommand
        {
            Id = id,
            ExpireDate = expireDate,
            IsPermanent = isPermanent,
            Description = description
        };
        try
        {
            await _hubClientService.UpdateBannedPlayerAsync(request);
            return new ApplicationResult { Success = true };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ApplicationResult> GetAll(PlayerFilter filter)
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


        return new ApplicationResult
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

    public async Task<ApplicationResult> GetById(int id)
    {
        var player = await _playerRepository
           .Query(x => x.Id == id)
           .Include(x => x.Segments)
           .FirstOrDefaultAsync();

        if (player == null)
            throw new NotFoundException("Player Not Found!");

        var transactions = await _transactionRepository.Query(x => x.PlayerId == player.Id)
            .Include(x => x.Status)
            .ToListAsync();

        var totalCount = transactions.Count;

        var res = transactions.Select(x => new TransactionDto
        {
            Id = x.Id,
            Amount = x.Amount,
            Status = x.Status?.Name,
        }).ToList();

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

        var logs = await _playerLogRepository.Query(x => x.PlayerId == player.Id).Include(x => x.PlayerLogType).ToListAsync();

        var result = new PlayerDto
        {
            Id = player.Id,
            PlayerName = player.UserName,
            IsBanned = player.IsBanned,
            Segments = player.Segments.Select(x => new SegmentListDto
            {
                Id = x.Id,
                Description = x.Description,
            }).ToList(),
            Transactions = res,
            RegistrationDate = null,
            LastVisit = null,
            Referee = referee != null ? new RefereeDto
            {
                Id = referee.Id,
                UserName = refPlayer?.UserName,
                InvitedDateTime = referee.DateCreated
            } : null,
            PlayerLogs = logs.Select(x => new PlayerLogDto
            {
                Id = x.Id,
                Log = x.Log,
                TimeStamp = x.Timestamp,
                PlayerLogType = x.PlayerLogType?.Name,
            }).ToList(),
            Referrals = referrals
        };

        return new ApplicationResult
        {
            Data = result,
            Success = true,
        };
    }

    public async Task<ApplicationResult> GetBalance(int id)
    {
        var balances = _playerBalanceRepository.Query(x => x.PlayerId == id);

        var result = balances.Select(x => new PlayerBalanceDto
        {
            Id = x.Id,
            Amount = x.Amount,
            Currency = x.Coin.Name,
        });

        return new ApplicationResult
        {
            Success = true,
            Data = await result.ToListAsync()
        };
    }

    public async Task<ApplicationResult> GetBannedPlayer(int id)
    {
        var palyer = await _playerBanRepository.Query(x => x.Id == id).FirstOrDefaultAsync();

        return new ApplicationResult
        {
            Success = true,
            Data = palyer ?? null,
        };
    }

    public async Task<ApplicationResult> GetAllBannedPlayers()
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


        return new ApplicationResult
        {
            Success = true,
            Data = await result.ToListAsync()
        };
    }

    public async Task<ApplicationResult> GetLeaderBoardResultByPlayer(int playerId)
    {
        var leaderboardResults = _leaderboardResultRepository.Query().Where(x => x.PlayerId == playerId);

        var total = leaderboardResults.Count();

        return new ApplicationResult
        {
            Success = true,
            Data = await leaderboardResults.ToListAsync(),
        };
    }

    public async Task<ApplicationResult> GetPlayerProgress(int id)
    {
        var progress = await _playerProgressRepository.Query(x => x.PlayerId == id).FirstOrDefaultAsync();

        var result = new PlayerProgressDto
        {
            DailyProgress = progress?.Progress ?? 0,
            TotalProgress = progress?.Progress ?? 0
        };

        return new ApplicationResult
        {
            Success = true,
            Data = result
        };
    }
}
