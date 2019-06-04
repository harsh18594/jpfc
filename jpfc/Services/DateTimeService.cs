using jpfc.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jpfc.Services
{
    public class DateTimeService : IDateTimeService
    {
        public TimeZoneInfo FetchTimeZoneInfo(string timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        public DateTime ConvertUtcToDateTime(DateTime utcDateTime, TimeZoneInfo timeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZone);
        }
    }
}
