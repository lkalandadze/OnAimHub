using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;
using OnAim.Admin.Shared.DTOs.Player;

namespace OnAim.Admin.APP.Queries.Player.GetAll
{
    public sealed record GetAllPlayerQuery(
       PlayerFilter Filter
        ) : IQuery<ApplicationResult>;
}
