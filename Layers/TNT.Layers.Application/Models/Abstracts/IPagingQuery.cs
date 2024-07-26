namespace TNT.Layers.Application.Models.Abstracts
{
    public interface IPagingQuery
    {
        int Skip { get; }
        int? Take { get; }
        bool CanGetAll();
    }
}
