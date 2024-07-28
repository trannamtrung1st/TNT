namespace TNT.Layers.Services
{
    public static class EnvironmentDefaults
    {
        public static class AspNetCoreEnvs
        {
            public const string Key = "ASPNETCORE_ENVIRONMENT";
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
        }
    }

    internal static class RequestContextKeys
    {
        public const string DisplayName = nameof(DisplayName);
        public const string IdentityId = nameof(IdentityId);
    }

    internal static class SwaggerDefaults
    {
        public const string Prefix = "swagger";
        public const string DocEndpointFormat = "/swagger/{0}/swagger.json";
    }

    internal static class ApiDefaults
    {
        public const string VersionGroupNameFormat = "'v'VVV";
    }

    internal static class Messages
    {
        public static class Welcome
        {
            public const string SwaggerInstruction = "Please go to configured Swagger endpoint for API documentation.";
        }
    }
}
