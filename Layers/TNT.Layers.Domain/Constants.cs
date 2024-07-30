﻿namespace TNT.Layers.Domain
{
    public enum DomainEventType
    {
        PrePersisted = 1,
        PostPersisted = 2
    }

    public static class DateTimeFormats
    {
        public const string DDMMYYYY = "dd/MM/yyyy";
        public const string MMDDYYYY = "MM/dd/yyyy";
        public const string YYYYMMDD = "yyyy/MM/dd";
        public const string YYYYMMDD_HYPHEN = "yyyy-MM-dd";
        public const string FULL_DATE_TIME = "dddd, dd MMMM yyyy HH:mm:ss";
        public const string SHORT_DATE_TIME = "MM/dd/yyyy HH:mm";
        public const string TIME_12_HOUR = "hh:mm tt";
        public const string TIME_24_HOUR = "HH:mm";
        public const string MONTH_DAY_YEAR = "MMMM dd, yyyy";
        public const string MONTH_YEAR = "MMMM yyyy";
        public const string ISO_8601 = "yyyy-MM-ddTHH:mm:ss.fffK";
        public const string RFC1123 = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
        public const string UNIVERSAL_SORTABLE = "yyyy-MM-dd HH:mm:ssZ";
    }

    public static class ResultCodes
    {
        public static class Common
        {
            public const string Prefix = "common/";
            public const string ObjectResult = Prefix + "object";
            public const string NotFound = Prefix + "not_found";
            public const string BadRequest = Prefix + "bad_request";
            public const string AccessDenied = Prefix + "access_denied";
            public const string UnknownError = Prefix + "unknown_error";
            public const string PersistenceError = Prefix + "persistence_error";
            public const string OperationFailed = Prefix + "operation_failed";
        }
    }

    public static class ErrorCodes
    {
        public static class Common
        {
            private const string Prefix = "common/";
            public const string Empty = Prefix + "empty";
            public const string Null = Prefix + "null";
            public const string AboveMax = Prefix + "above_max";
            public const string UnderMin = Prefix + "under_min";
            public const string OutOfRange = Prefix + "out_of_range";
            public const string InvalidFormat = Prefix + "invalid_format";
            public const string InvalidValue = Prefix + "invalid_value";
        }
    }
}
