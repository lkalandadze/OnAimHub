using Hub.Domain.Entities;

namespace Hub.Application.Features.PlayerFeatures.Dtos;

public class PlayerBaseDtoModel
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public static PlayerBaseDtoModel MapFrom(Player player)
    {
        return new PlayerBaseDtoModel
        {
            Id = player.Id,
            UserName = player.UserName,
        };
    }
}