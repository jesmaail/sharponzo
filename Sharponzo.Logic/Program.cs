﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Sharponzo.MonzoTypes;

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


            var startOfWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);
            var dateWithTransaction = new DateTime(2017, 05, 08);

            var paymentsThisWeek = GetPaymentsByDate(startOfWeek, endOfWeek);
            var paymentsOnDate = GetPaymentsByDate(dateWithTransaction);
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

        private static IEnumerable<Transaction> GetPaymentsByDate(DateTime start, DateTime end)
        {
            return GetAllPayments()
                .Where(payment => payment.Date > start && payment.Date < end).ToList();
        }

        private static IEnumerable<Transaction> GetPaymentsByDate(DateTime date)
        {
            return GetAllPayments()
                .Where(payment => payment.Date.Date == date.Date).ToList();
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

        // Need to think about doing it as web app, Monzo requires a web kickback
        //public static async Task<string> Authentication()
        //{
        //    var clientId = "";
        //    var clientSecret = "";
        //    var baseUrl = "";
        //    var accessToken = "";

        //    // GetAccessToken (async task)
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseUrl);

        //        // Set Response to be JSON
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // Set data to POST
        //        var dataToPost = new List<KeyValuePair<string, string>>();
        //        dataToPost.Add(new KeyValuePair<string, string>("grantType", "client_credentials"));
        //        dataToPost.Add(new KeyValuePair<string, string>("clientId", clientId));
        //        dataToPost.Add(new KeyValuePair<string, string>("clientSecret", clientSecret));
        //        var content = new FormUrlEncodedContent(dataToPost);
                
        //        // Post the data and get response
        //        var response = await client.PostAsync("Token", content);
        //        var jsonString = await response.Content.ReadAsStringAsync();
        //        var responseData = JsonConvert.DeserializeObject(jsonString);

        //        return ((dynamic) responseData).access_token;
        //    }
        //}
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
    }
}
