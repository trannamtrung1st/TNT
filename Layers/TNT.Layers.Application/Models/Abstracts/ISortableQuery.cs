namespace TNT.Layers.Application.Models.Abstracts
{
    public interface ISortableQuery<T>
    {
        T SortBy { get; }
        bool IsDesc { get; }
    }
}
