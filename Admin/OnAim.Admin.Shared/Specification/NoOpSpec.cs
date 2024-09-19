using System.Linq.Expressions;

namespace OnAim.Admin.Shared.Specification
{
    public class NoOpSpec<TEntity> : SpecificationBase<TEntity>
    {
        public override Expression<Func<TEntity, bool>> Criteria => p => true;
    }
}
