using Microsoft.EntityFrameworkCore.Metadata;

namespace TNT.Layers.Persistence.QueryFilters.Abstracts
{
    public interface IQueryFilterProvider
    {
        string ProvideMethodName { get; }
        bool CanApply(IMutableEntityType eType);
    }
}
