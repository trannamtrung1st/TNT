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

    public static class SwaggerDefaults
    {
        public const string RouteTemplate = "/swagger/{documentName}/swagger.{extension:regex(^(json|ya?ml)$)}";
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

    // Reference: https://github.com/openiddict/openiddict-core/blob/1f630eb4d8f18d52d373ebaec02ebbb9e47760e9/src/OpenIddict.Abstractions/OpenIddictConstants.cs#L63
    public static class Claims
    {
        public static class Prefixes
        {
            public const string Private = "oi_";
        }

        public static class Private
        {
            public const string AccessTokenLifetime = "oi_act_lft";

            public const string Audience = "oi_aud";

            public const string AuthorizationCodeLifetime = "oi_auc_lft";

            public const string AuthorizationId = "oi_au_id";

            public const string ClaimDestinationsMap = "oi_cl_dstn";

            public const string CodeChallenge = "oi_cd_chlg";

            public const string CodeChallengeMethod = "oi_cd_chlg_meth";

            public const string CodeVerifier = "oi_cd_vrf";

            public const string CreationDate = "oi_crt_dt";

            public const string DeviceCodeId = "oi_dvc_id";

            public const string DeviceCodeLifetime = "oi_dvc_lft";

            public const string EndpointType = "oi_ept_typ";

            public const string ExpirationDate = "oi_exp_dt";

            public const string GrantType = "oi_grt_typ";

            public const string HostProperties = "oi_hst_props";

            public const string IdentityTokenLifetime = "oi_idt_lft";

            public const string InstanceId = "oi_instc_id";

            public const string Issuer = "oi_iss";

            public const string Nonce = "oi_nce";

            public const string PostLogoutRedirectUri = "oi_pstlgt_reduri";

            public const string ProviderName = "oi_prvd_name";

            public const string Presenter = "oi_prst";

            public const string RedirectUri = "oi_reduri";

            public const string RefreshTokenLifetime = "oi_reft_lft";

            public const string RegistrationId = "oi_reg_id";

            public const string Resource = "oi_rsrc";

            public const string ResponseType = "oi_rsp_typ";

            public const string SigningAlgorithm = "oi_sign_alg";

            public const string Scope = "oi_scp";

            public const string StateTokenLifetime = "oi_stet_lft";

            public const string TokenId = "oi_tkn_id";

            public const string TokenType = "oi_tkn_typ";

            public const string UserCodeLifetime = "oi_usrc_lft";
        }

        public const string AccessTokenHash = "at_hash";

        public const string Active = "active";

        public const string Address = "address";

        public const string Audience = "aud";

        public const string AuthenticationContextReference = "acr";

        public const string AuthenticationMethodReference = "amr";

        public const string AuthenticationTime = "auth_time";

        public const string AuthorizationServer = "as";

        public const string AuthorizedParty = "azp";

        public const string Birthdate = "birthdate";

        public const string ClientId = "client_id";

        public const string CodeHash = "c_hash";

        public const string Country = "country";

        public const string Email = "email";

        public const string EmailVerified = "email_verified";

        public const string ExpiresAt = "exp";

        public const string FamilyName = "family_name";

        public const string Formatted = "formatted";

        public const string Gender = "gender";

        public const string GivenName = "given_name";

        public const string IssuedAt = "iat";

        public const string Issuer = "iss";

        public const string Locale = "locale";

        public const string Locality = "locality";

        public const string JwtId = "jti";

        public const string KeyId = "kid";

        public const string MiddleName = "middle_name";

        public const string Name = "name";

        public const string Nickname = "nickname";

        public const string Nonce = "nonce";

        public const string NotBefore = "nbf";

        public const string PhoneNumber = "phone_number";

        public const string PhoneNumberVerified = "phone_number_verified";

        public const string Picture = "picture";

        public const string PostalCode = "postal_code";

        public const string PreferredUsername = "preferred_username";

        public const string Profile = "profile";

        public const string Region = "region";

        public const string RequestForgeryProtection = "rfp";

        public const string Role = "role";

        public const string Scope = "scope";

        public const string StreetAddress = "street_address";

        public const string Subject = "sub";

        public const string TargetLinkUri = "target_link_uri";

        public const string TokenType = "token_type";

        public const string TokenUsage = "token_usage";

        public const string UpdatedAt = "updated_at";

        public const string Username = "username";

        public const string Website = "website";

        public const string Zoneinfo = "zoneinfo";
    }
}
