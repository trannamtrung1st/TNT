using System.Data;

namespace TNT.Layers.Persistence.Services.Abstracts
{
    public interface IDbConnectionProvider
    {
        IDbConnection CreateConnection();
    }

    public interface IDbConnectionProvider<TArgs>
    {
        IDbConnection CreateConnection(TArgs args);
    }
}
