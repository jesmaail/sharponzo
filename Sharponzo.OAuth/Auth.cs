using System;
using RestSharp;

namespace Sharponzo.OAuth
{
    public class Auth
    {
        private IRestClient _client;
        private const string BASE_URI = "http://auth.monzo.com";
        // This cannot be committed, needs to live on machine/confg
        private const string CLIENT_ID = "";         
        // This should also live in config
        private const string REDIRECT_URI = "http://localhost:5000";
        private const string RESPONSE_TYPE = "code";

        public Auth() => _client = new RestClient(BASE_URI);

        public void Authenticate()
        {
            //"https://auth.monzo.com/?client_id=$client_id&redirect_uri=$redirect_uri&response_type=code&state=$state_token"

            var request = new RestRequest();
            request.AddParameter("client_id", CLIENT_ID);
            request.AddParameter("redirect_uri", REDIRECT_URI);
            request.AddParameter("response_type", RESPONSE_TYPE);

            var response = _client.Execute(request);

            var content = response.Content;
        }
    }
}
