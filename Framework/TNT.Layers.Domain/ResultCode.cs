namespace TNT.Layers.Domain
{
    public static class ResultCode
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
}
