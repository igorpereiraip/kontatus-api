using System;

namespace Kontatus.Helper.Utilitarios
{
    public static class TimezoneConfig
    {
        public static string UserTimezone { get; set; } = "E. South America Standard Time";

        public static DateTime ConvertDateTime(DateTime datetime) 
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(UserTimezone);
            return TimeZoneInfo.ConvertTime(datetime, TimeZoneInfo.Utc, timeZoneInfo);
        }

        public static DateTime? ConvertDateTime(DateTime? datetime)
        {
            if (datetime.HasValue == false) {
                return datetime;
            }

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(UserTimezone);
            return TimeZoneInfo.ConvertTime(datetime.Value, TimeZoneInfo.Utc, timeZoneInfo);
        }

        public static DateTime? ConvertDateTimeToUTC(DateTime? datetime) 
        {
            if (datetime.HasValue == false)
            {
                return datetime;
            }

            return datetime.Value.ToUniversalTime();
        }
    }
}
