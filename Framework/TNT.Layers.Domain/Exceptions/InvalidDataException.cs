namespace TNT.Layers.Domain.Exceptions
{
    public class InvalidDataException : BaseException
    {
        public InvalidDataException(params string[] properties)
            : base(ResultCode.Common.InvalidData, data: properties)
        {
        }
    }
}
