using OnAim.Admin.APP.Queries.Abstract;
using OnAim.Admin.Shared.ApplicationInfrastructure;

namespace OnAim.Admin.APP.Queries.User.GetById
{
    public sealed record GetUserByIdQuery(int Id) : IQuery<ApplicationResult>;
}
