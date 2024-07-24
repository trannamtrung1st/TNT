namespace TNT.Layers.Service.Services.Abstracts
{
    public interface IRequestContext
    {
        void Set(string key, object value);
        object Get(string key);
    }
}