using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sharponzo.Logic
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

        public async Task<string> GetAccounts()
        {
            var response = await _httpClient.GetAsync($"/accounts");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception();
            }

            return body;
        }
    }
}