using System;
using System.Text;

namespace Sharponzo
{ 
    public class Program
    {
        private const int READLINE_BUFFER_SIZE = 290;

        public static void Main()
        {
            Console.WriteLine("Please enter Monzo Access Token: ");
            var access = ReadLine();
            var api = new ApiLayer(access);

            var accounts = api.GetAccounts().Result;
            var accountId = accounts[0].Id;

            var transactions = api.GetTransactions(accountId).Result;

            var balance = api.GetBalance(accountId).Result;

        }

        private static void GetAllPayments()
        {
            throw new NotImplementedException();
        }

        private static void GetAllTopups()
        {
            throw new NotImplementedException();
        }

        private static void GetCurrentBalance()
        {
            throw new NotImplementedException();
        }

        private static void GetAccountHolder()
        {
            throw new NotImplementedException();
        }

        private static void GetAllMerchantNames()
        {
            throw new NotImplementedException();
        }

        private static void GetCategories()
        {
            throw new NotImplementedException();
        }

        private static void GetPaymentsByMerchant(string merchantName)
        {
            throw new NotImplementedException();   
        }

        private static void GetPaymentsByCategory(string category)
        {
            throw new NotImplementedException();
        }

        private static void GetPaymentsByDate(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        private static void GetPaymentsByDate(DateTime date)
        {
            throw new NotImplementedException();
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
}
