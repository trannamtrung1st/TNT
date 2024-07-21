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

    public static class ResultCodes
    {
        public static class Common
        {
            public const string Prefix = "common";
            public const string ObjectResult = Prefix + "_object";
            public const string NotFound = Prefix + "_notfound";
            public const string BadRequest = Prefix + "_badrequest";
            public const string AccessDenied = Prefix + "_accessdenied";
            public const string UnknownError = Prefix + "_unknownerror";
            public const string PersistenceError = Prefix + "_persistenceerror";
            public const string InvalidPagination = Prefix + "_invalidpagination";
            public const string InvalidData = Prefix + "_invaliddata";
        }
    }

    public static class ErrorCodes
    {
        public static class Common
        {
            public const string Prefix = "common";
            public const string Empty = Prefix + "_empty";
            public const string Max = Prefix + "_max";
            public const string Min = Prefix + "_min";
            public const string Format = Prefix + "_format";
        }
    }
}
