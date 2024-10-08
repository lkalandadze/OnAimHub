#nullable disable

namespace GameLib.Application.Models.PrizeType;

public class PrizeTypeBaseGetModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsMultiplied { get; set; }

    public static PrizeTypeBaseGetModel MapFrom(Domain.Entities.PrizeType prizeType)
    {
        return PrizeTypeGetModel.MapFrom(prizeType, false);
    }
}