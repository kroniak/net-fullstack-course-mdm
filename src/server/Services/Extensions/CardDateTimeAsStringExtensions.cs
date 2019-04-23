using System;
using System.Globalization;

namespace Server.Services.Extensions
{
    public static class CardDateTimeAsStringExtensions
    {
        /// <summary>
        /// Convert DateTime and years to Exp date "01/18"
        /// </summary>
        /// <param name="dt">dateTime card</param>
        /// <param name="years">count of years</param>
        public static string ToShortStringFormat(this DateTime dt, int years) =>
            dt.AddYears(years).ToString("MM/yy", CultureInfo.GetCultureInfo("en-us"));
    }
}