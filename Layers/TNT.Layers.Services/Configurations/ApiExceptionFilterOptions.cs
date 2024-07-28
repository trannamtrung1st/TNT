namespace TNT.Layers.Services.Configurations
{
    public class ApiExceptionFilterOptions
    {
        public const int DefaultMaxBodyLength = 10_000;
        public int MaxBodyLengthForLogging { get; set; } = DefaultMaxBodyLength;
    }
}