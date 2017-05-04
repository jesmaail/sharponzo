using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sharponzo.MonzoTypes;

namespace Sharponzo
{
    public class ApiLayer
    {
        private string _accessToken;
        private readonly HttpClient _httpClient;

        private const string BASE_URI = "https://api.monzo.com";

        public ApiLayer(string accessToken)
        {
            _accessToken = accessToken;
            _httpClient = new HttpClient{BaseAddress = new Uri(BASE_URI)};

            if (_httpClient == null) throw new Exception();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<IList<Account>> GetAccounts()
        {
            var response = await _httpClient.GetAsync($"/accounts");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountList>(body).Accounts;
        }

        public async Task<string> WhoAmI()
        {
            var response = await _httpClient.GetAsync($"/ping/whoami");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IList<Transaction>> GetTransactions(string accountId)
        {
            var response = await _httpClient.GetAsync($"/transactions?expand[]=merchant&account_id=" + accountId);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TransactionList>(body).Transactions;

            //return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetBalance(string accountId)
        {
            var response = await _httpClient.GetAsync($"/balance?account_id=" + accountId);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}