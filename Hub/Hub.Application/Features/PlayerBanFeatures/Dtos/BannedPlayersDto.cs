using Hub.Domain.Entities;

namespace Hub.Application.Features.PlayerBanFeatures.Dtos;

public class BannedPlayersDto
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public DateTimeOffset DateBanned { get; set; }
    public DateTimeOffset? ExpireDate { get; set; }
    public bool IsPermanent { get; set; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset? RevokeDate { get; set; }
    public string Description { get; set; }


    public static BannedPlayersDto MapFrom(PlayerBan playerBan)
    {
        return new BannedPlayersDto
        {
            Id = playerBan.Id,
            PlayerId = playerBan.PlayerId,
            DateBanned = playerBan.DateBanned,
            ExpireDate = playerBan.ExpireDate,
            IsPermanent = playerBan.IsPermanent,
            IsRevoked = playerBan.IsRevoked,
            RevokeDate = playerBan.RevokeDate,
            Description = playerBan.Description
        };
    }
}