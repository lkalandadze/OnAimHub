using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.Player.GetById
{
    public sealed record GetPlayerByIdQuery(int Id) : IQuery<ApplicationResult>;
}
