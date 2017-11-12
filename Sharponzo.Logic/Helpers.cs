using System;
using System.Collections.Generic;
using System.Text;

namespace Sharponzo
{
    public static class Helpers
    {
        private const int READLINE_BUFFER_SIZE = 290;

        // Required as the Monzo Access Token is massive!
        public static string ReadLine()
        {
            var inputStream = Console.OpenStandardInput(READLINE_BUFFER_SIZE);
            var bytes = new byte[READLINE_BUFFER_SIZE];
            inputStream.Read(bytes, 0, READLINE_BUFFER_SIZE);

            // The Carriage return gets included in the resulting string, so needs to be removed
            var result = Encoding.Default.GetString(bytes);
            return result.Substring(0, result.Length - 2);
        }
    }

    // Needs some refactoring
    // Also need a similar way to take the start and end of a given month
    // Have it pass in the DateTime dt also to get the start of any given week.
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static IEnumerable<DateTime> GetWeeksForMonth(this DateTime month, DayOfWeek startOfWeek)
        {
            var weeks = new List<DateTime>();
            var startOfMonth = new DateTime(month.Year, month.Month, 1);

            var current = month.Month;
            var week = startOfMonth.StartOfWeek(startOfWeek);

            while (current == month.Month)
            {
                weeks.Add(week);
                week = week.AddDays(7);
                current = week.Month;
            }

            return weeks;
        }
    }
}
