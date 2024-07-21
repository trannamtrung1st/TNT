namespace TNT.Layers.Domain
{
    public enum DomainEventType
    {
        PrePersisted = 1,
        PostPersisted = 2
    }

    public static class DateTimeFormat
    {
        public const string DDMMYYYY = "dd/MM/yyyy";
    }

    public static class CommonConstraints
    {
        public const int MaxStringLength = 255;
        public const int IdMaxLength = 50;
    }
}
