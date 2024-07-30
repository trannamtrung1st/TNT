namespace TNT.Boilerplates.Http
{
    public static class MediaTypes
    {
        // Application types
        public const string ApplicationJson = "application/json";
        public const string ApplicationXml = "application/xml";
        public const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
        public const string ApplicationPdf = "application/pdf";
        public const string ApplicationZip = "application/zip";
        public const string ApplicationOctetStream = "application/octet-stream";

        // Image types
        public const string ImagePng = "image/png";
        public const string ImageJpeg = "image/jpeg";
        public const string ImageGif = "image/gif";
        public const string ImageBmp = "image/bmp";
        public const string ImageSvgXml = "image/svg+xml";

        // Text types
        public const string TextPlain = "text/plain";
        public const string TextHtml = "text/html";
        public const string TextCss = "text/css";
        public const string TextJavascript = "text/javascript";

        // Audio types
        public const string AudioMpeg = "audio/mpeg";
        public const string AudioOgg = "audio/ogg";
        public const string AudioWav = "audio/wav";

        // Video types
        public const string VideoMp4 = "video/mp4";
        public const string VideoMpeg = "video/mpeg";
        public const string VideoOgg = "video/ogg";

        // Multipart types
        public const string MultipartFormData = "multipart/form-data";
    }

    public static class XHeaderNames
    {
        public const string XForwardedHost = "X-Forwarded-Host";
        public const string XForwardedFor = "X-Forwarded-For";
        public const string XForwardedProto = "X-Forwarded-Proto";
        public const string XRealIP = "X-Real-IP";
        public const string XOriginalURL = "X-Original-URL";
        public const string XRequestID = "X-Request-ID";
        public const string XCorrelationID = "X-Correlation-ID";
        public const string XForwardedPort = "X-Forwarded-Port";
        public const string XContentTypeOptions = "X-Content-Type-Options";
        public const string XFrameOptions = "X-Frame-Options";
        public const string XXSSProtection = "X-XSS-Protection";
        public const string XUACompatible = "X-UA-Compatible";
        public const string XPoweredBy = "X-Powered-By";
        public const string XPermittedCrossDomainPolicies = "X-Permitted-Cross-Domain-Policies";
        public const string XRobotsTag = "X-Robots-Tag";
    }
}