namespace TNT.Layers.Domain.Exceptions
{
    public class PersistenceException<T> : TypedDataException<T>
    {
        public PersistenceException(T data) : base(ResultCode.Common.PersistenceError, data: data)
        {
        }
    }

    public class PersistenceException : PersistenceException<object>
    {
        public PersistenceException(object data) : base(data)
        {
        }
    }
}
