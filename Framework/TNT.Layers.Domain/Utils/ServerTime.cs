using System;
using TimeZoneConverter;

namespace TNT.Layers.Domain.Utils
{
    public static class ServerTime
    {
        public const string DefaultTimeZoneId = "SE Asia Standard Time";
        public static readonly TimeZoneInfo DefaultTimeZone = TZConvert.GetTimeZoneInfo(DefaultTimeZoneId);

        public static DateTime Now => DateTime.SpecifyKind(TimeZoneInfo
            .ConvertTime(DateTime.Now, DefaultTimeZone), DateTimeKind.Local);
    }
}
