using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Sharponzo.MonzoTypes;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Sharponzo
{ 
    public class Program
    {
        // TODO:
        //  - Unit test <Monzo> & <Filter> Objects
        //      - This can eliminate a lot of the code from Main()
        //      - Use NUnit (need to NuGet that)

        //  - <Calculator> for the commented out methods of
        //      - SpendForDay()
        //      - HighestSpendingDay()

        //  - <PrettyPrint> for the commented out method PrintStatementForMonth()
        //      - Also want more methods in this one of course

        //  - Handle the filters/requests/etc. in the command line rather than hard coding them!
        //      - Prettify the outputs of these requests
        //      - Help option to show available commands
        //      - Moving existing ones into unit tests will aid this

        //  - Split out the MonzoAccessToken logic
        //      - Will help reuse this code as the logic-side of a web application!

        //  - Improve Class naming!

        public static void Main()
        {
            Console.WriteLine("Please enter Monzo Access Token: ");
            var access = Helpers.ReadLine();
            var monzoApi = new ApiLayer(access);

            var monzo = new Monzo(monzoApi);
            var payments = monzo.GetAllPayments();
            var topups = monzo.GetAllTopups();
            var currentBalance = monzo.GetCurrentBalance();
            var accountHolder = monzo.GetAccountHolder();
            var merchantNames = monzo.GetMerchantNames();
            var categories = monzo.GetCategories();

            var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);
            var dateWithTransaction = new DateTime(2017, 05, 08);

            var filter = new Filter(payments);
            var tescoPayments = filter.GetPaymentsByMerchant("tesco");
            var groceryPayments = filter.GetPaymentsByCategory("groceries");
            var paymentsThisWeek = filter.GetPaymentsByDateRange(startOfWeek, endOfWeek);
            var paymentsOnDate = filter.GetPaymentsByDate(dateWithTransaction);

            var spentThisWeek = GetPaymentsAmount(paymentsThisWeek);

            //PrintStatementForMonth(06, 2017);
            //var weekBiggestSpend = new DateTime(2017, 06, 05);
            //var highestSpendDay = HighestSpendDayPerWeek(weekBiggestSpend);
            //var highestSpendAmount = GetPaymentsAmount(filter.GetPaymentsByDate(highestSpendDay));
            //var formattedSpendAmount = $"{highestSpendAmount:C2}";

            //Console.WriteLine("Biggest spending day for week commencing " + weekBiggestSpend.ToString("dd MMM") + ": ");
            //Console.WriteLine(highestSpendDay.ToString("dddd MMM") + ": " + formattedSpendAmount);
            var pause = Helpers.ReadLine();
        }

        //private static void PrintStatementForMonth(int month, int year)
        //{
        //    var periodWeeks = new DateTime(year, month, 01).GetWeeksForMonth(DayOfWeek.Monday);
        //    var periodPayments = new List<Transaction>();
        //    foreach (var week in periodWeeks)
        //    {
        //        var paymentsForWeek = GetPaymentsByWeek(week);
        //        periodPayments.AddRange(paymentsForWeek);

        //        var amountForWeek = GetPaymentsAmount(paymentsForWeek);
        //        Console.WriteLine(week.ToString("dd MMM") + " - " + week.AddDays(6).ToString("dd MMM") + ": " + amountForWeek);
        //    }
        //    var periodSpend = GetPaymentsAmount(periodPayments);
        //    var formattedSpend = $"{periodSpend:C2}";
        //    Console.WriteLine("Total for period: " + formattedSpend);
        //}

        //private static DateTime HighestSpendDayPerWeek(DateTime startOfWeek)
        //{
        //    var max = 0.0;
        //    var dayIndex = 0;

        //    for (var i = 0; i < 7; i++)
        //    {
        //        var spend = GetSpendForDay(startOfWeek.AddDays(i));
        //        if (!(spend > max)) continue;
        //        max = spend;
        //        dayIndex = i;
        //    }

        //    return startOfWeek.AddDays(dayIndex);
        //}

        //private static double GetSpendForDay(DateTime day)
        //{
        //    var transactions = GetPaymentsByDate(day);
        //    var amount = GetPaymentsAmount(transactions);

        //    return amount;
        //}

        private static double GetPaymentsAmount(IEnumerable<Transaction> transactions)
        {
            double amount = transactions.Sum(transaction => transaction.Amount);
            amount = Math.Abs(amount);
            amount = amount / 100;

            return amount;
        }      
    }    
}
