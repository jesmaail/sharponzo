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
        private const int READLINE_BUFFER_SIZE = 290;

        private static ApiLayer _api;

        private static string _accountId;
        private static IList<Account> _accounts;
        private static IList<Transaction> _transactions;
        private static Balance _balance;

        // TODO:
        //  - Split out the logic in here into seperate objects!
        //      - <Filter> object to do the GetPaymentsBy methods
        //      - <Monzo> object to handle GetAllPayments() GetAllTopups() etc.

        //  - Handle the filters/requests/etc. in the command line rather than hard coding them!
        //      - Prettify the outputs of these requests
        //      - Help option to show available commands

        //  - Split out (and refactor) the DateTime extension

        //  - Split out the MonzoAccessToken logic and ReadLine()
        //      - Will help reuse this code as the logic-side of a web application!
        public static void Main()
        {
            Console.WriteLine("Please enter Monzo Access Token: ");
            var access = ReadLine();
            _api = new ApiLayer(access);

            _accounts = _api.GetAccounts().Result;
            _accountId = _accounts[0].Id;
            _transactions = _api.GetTransactions(_accountId).Result;
            _balance = _api.GetBalance(_accountId).Result;

            var payments = GetAllPayments();
            var topups = GetAllTopups();
            var currentBalance = GetCurrentBalance();

            var accountHolder = GetAccountHolder();
            var merchantNames = GetAllMerchantNames();
            var categories = GetCategories();

            var tescoPayments = GetPaymentsByMerchant("tesco");
            var groceryPayments = GetPaymentsByCategory("groceries");

            //var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            //var endOfWeek = startOfWeek.AddDays(6);
            //var dateWithTransaction = new DateTime(2017, 05, 08);
            //var paymentsThisWeek = GetPaymentsByDate(startOfWeek, endOfWeek);
            //var paymentsOnDate = GetPaymentsByDate(dateWithTransaction);
            //var spentThisWeek = GetPaymentsAmount(paymentsThisWeek);
            
            PrintStatementForMonth(06, 2017);
            var weekBiggestSpend = new DateTime(2017, 06, 05);
            var highestSpendDay = HighestSpendDayPerWeek(weekBiggestSpend);
            var highestSpendAmount = GetPaymentsAmount(GetPaymentsByDate(highestSpendDay));
            var formattedSpendAmount = $"{highestSpendAmount:C2}";

            Console.WriteLine("Biggest spending day for week commencing " + weekBiggestSpend.ToString("dd MMM") + ": ");
            Console.WriteLine(highestSpendDay.ToString("dddd MMM") + ": " + formattedSpendAmount);
            var pause = ReadLine();
        }

        private static void PrintStatementForMonth(int month, int year)
        {
            var periodWeeks = new DateTime(year, month, 01).GetWeeksForMonth(DayOfWeek.Monday);
            var periodPayments = new List<Transaction>();
            foreach (var week in periodWeeks)
            {
                var paymentsForWeek = GetPaymentsByWeek(week);
                periodPayments.AddRange(paymentsForWeek);

                var amountForWeek = GetPaymentsAmount(paymentsForWeek);
                Console.WriteLine(week.ToString("dd MMM") + " - " + week.AddDays(6).ToString("dd MMM") + ": " + amountForWeek);
            }
            var periodSpend = GetPaymentsAmount(periodPayments);
            var formattedSpend = $"{periodSpend:C2}";
            Console.WriteLine("Total for period: " + formattedSpend);
        }

        private static DateTime HighestSpendDayPerWeek(DateTime startOfWeek)
        {
            var max = 0.0;
            var dayIndex = 0;

            for (var i = 0; i < 7; i++)
            {
                var spend = GetSpendForDay(startOfWeek.AddDays(i));
                if (!(spend > max)) continue;
                max = spend;
                dayIndex = i;
            }

            return startOfWeek.AddDays(dayIndex);
        }

        private static double GetSpendForDay(DateTime day)
        {
            var transactions = GetPaymentsByDate(day);
            var amount = GetPaymentsAmount(transactions);

            return amount;
        }

        private static double GetPaymentsAmount(IEnumerable<Transaction> transactions)
        {
            double amount = transactions.Sum(transaction => transaction.Amount);
            amount = Math.Abs(amount);
            amount = amount / 100;

            return amount;
        }

        private static IEnumerable<Transaction> GetAllPayments()
        {
            return _transactions.Where(transaction => !transaction.IsLoad).ToList();
        }

        private static IEnumerable<Transaction> GetAllTopups()
        {
            return _transactions.Where(transaction => transaction.IsLoad).ToList();
        }

        private static string GetCurrentBalance()
        {
            var value = (double)_balance.Amount / 100;
            return value.ToString("C", CultureInfo.CurrentCulture);
        }

        private static string GetAccountHolder()
        {
            return _accounts[0].Name;
        }

        private static IEnumerable<string> GetAllMerchantNames()
        {
            var result = new List<string>();
            var merchants = GetAllPayments().Select(payment => payment.Merchant.Name).ToList();
            result.AddRange(merchants.Distinct());
            return result;
        }

        private static IEnumerable<string> GetCategories()
        {
            var result = new List<string>();
            var categories = GetAllPayments().Select(payment => payment.Category).ToList();
            result.AddRange(categories.Distinct());
            return result;
        }

        private static IEnumerable<Transaction> GetPaymentsByMerchant(string merchantName)
        {
            return GetAllPayments()
                .Where(payment => string.Equals(
                    payment.Merchant.Name, 
                    merchantName,
                    StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        private static IEnumerable<Transaction> GetPaymentsByCategory(string category)
        {
            return GetAllPayments()
                .Where(payment => string.Equals(
                    payment.Category, 
                    category, 
                    StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }

        // Date Range
        private static IEnumerable<Transaction> GetPaymentsByDate(DateTime start, DateTime end)
        {
            return GetAllPayments()
                .Where(payment => payment.Date > start && payment.Date < end).ToList();
        }

        // Single Date
        private static IEnumerable<Transaction> GetPaymentsByDate(DateTime date)
        {
            return GetAllPayments()
                .Where(payment => payment.Date.Date == date.Date).ToList();
        }

        // Week
        private static IEnumerable<Transaction> GetPaymentsByWeek(DateTime startOfWeek)
        {
            return GetPaymentsByDate(startOfWeek, startOfWeek.AddDays(6));
        }

        // Required as the Monzo Access Token is massive!
        private static string ReadLine()
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
