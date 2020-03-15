using System;
using System.Globalization;

namespace JudgeSystem.Web.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static double GetDifferenceInSeconds(this DateTime date, string otherDate, string dateFormat)
        {
            var secondDate = DateTime.ParseExact(otherDate, dateFormat, CultureInfo.InvariantCulture);
            TimeSpan difference = date - secondDate;
            return difference.TotalSeconds;
        }
    }
}
