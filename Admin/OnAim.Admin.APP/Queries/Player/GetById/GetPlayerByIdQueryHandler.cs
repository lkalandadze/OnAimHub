using Microsoft.EntityFrameworkCore;
using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Infrasturcture.HubEntities;
using OnAim.Admin.Infrasturcture.Repository.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;
using OnAim.Admin.Shared.DTOs.Player.Balance;
using OnAim.Admin.Shared.DTOs.Player.Log;
using OnAim.Admin.Shared.DTOs.Refer;
using OnAim.Admin.Shared.DTOs.Segment;
using OnAim.Admin.Shared.DTOs.Transaction;
using OnAim.Admin.Shared.Exceptions;

namespace OnAim.Admin.APP.Queries.Player.GetById
{
    public class GetPlayerByIdQueryHandler : IQueryHandler<GetPlayerByIdQuery, ApplicationResult>
    {
        private readonly IReadOnlyRepository<Infrasturcture.HubEntities.Player> _playerRepository;
        private readonly IReadOnlyRepository<Transaction> _transactionRepository;
        private readonly IReadOnlyRepository<ReferralDistribution> _referalRepository;
        private readonly IReadOnlyRepository<PlayerBalance> _playerBalanaceRepository;
        private readonly IReadOnlyRepository<PlayerLog> _playerLogRepository;

        public GetPlayerByIdQueryHandler(
            IReadOnlyRepository<Infrasturcture.HubEntities.Player> playerRepository,
            IReadOnlyRepository<Infrasturcture.HubEntities.Transaction> transactionRepository,
            IReadOnlyRepository<Infrasturcture.HubEntities.ReferralDistribution> referalRepository,
            IReadOnlyRepository<Infrasturcture.HubEntities.PlayerBalance> playerBalanaceRepository,
            IReadOnlyRepository<Infrasturcture.HubEntities.PlayerLog> playerLogRepository
            )
        {
            _playerRepository = playerRepository;
            _transactionRepository = transactionRepository;
            _referalRepository = referalRepository;
            _playerBalanaceRepository = playerBalanaceRepository;
            _playerLogRepository = playerLogRepository;
        }

        public async Task<ApplicationResult> Handle(GetPlayerByIdQuery request, CancellationToken cancellationToken)
        {
            var player = await _playerRepository
                .Query(x => x.Id == request.Id)
                .Include(x => x.PlayerSegments)
                    .ThenInclude(x => x.Segment)
                .FirstOrDefaultAsync(cancellationToken);

            if (player == null)
                throw new NotFoundException("Player Not Found!");

            var transactions = await _transactionRepository.Query(x => x.PlayerId == player.Id)
                .Include(x => x.Status) 
                .ToListAsync(cancellationToken);

            var totalCount = transactions.Count; 

            var res = transactions.Select(x => new TransactionDto
            {
                Id = x.Id,
                Amount = x.Amount,
                Status = x.Status?.Name,
            }).ToList(); 

            var referee = await _referalRepository.Query(x => x.ReferrerId == player.ReferrerId).FirstOrDefaultAsync(cancellationToken);

            var referrals = await _referalRepository
                .Query(x => x.ReferrerId == player.Id)
                .Select(x => new ReferralDto
                {
                    Id = x.Id,
                    InvitedDateTime = x.DateCreated,
                    UserName = x.Referral.UserName,
                })
                .ToListAsync(cancellationToken);

            Infrasturcture.HubEntities.Player refPlayer = null;
            if (referee != null)
            {
                refPlayer = await _playerRepository.Query(x => x.ReferrerId == referee.ReferrerId).FirstOrDefaultAsync(cancellationToken);
            }

            var balance = await _playerBalanaceRepository.Query(x => x.PlayerId == player.Id).ToListAsync(cancellationToken);
            var logs = await _playerLogRepository.Query(x => x.PlayerId == player.Id).ToListAsync(cancellationToken);

            var result = new PlayerDto
            {
                Id = player.Id,
                PlayerName = player.UserName,
                Segments = player.PlayerSegments.Select(x => new SegmentDto
                {
                    Id = x.Segment.Id,
                    Description = x.Segment.Description,
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
                PlayerBalances = balance.Select(x => new PlayerBalanceDto
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Currency = x.Currency?.Name,
                }).ToList(),
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

    }
}
