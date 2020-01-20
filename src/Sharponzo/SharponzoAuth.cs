using Newtonsoft.Json;
using RestSharp;
using Sharponzo.Models;

namespace Sharponzo
{
    public class SharponzoAuth
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;

        private const string MONZO_OAUTH = "https://api.monzo.com/oauth2/token";

        public SharponzoAuth(string id, string secret, string redirectUri)
        {
            _clientId = id;
            _clientSecret = secret;
            _redirectUri = redirectUri;
        }

        // Use this to redirect to Monzo Authorisation page
        public string GetAuthRedirectUrl(string redirectUri = null)
        {
            var redirect = string.IsNullOrWhiteSpace(redirectUri) ? _redirectUri : redirectUri;

            return $"https://auth.monzo.com/?client_id={_clientId}&redirect_uri={redirect}&response_type=code";
        }

        // Call this from Callback location to get access code after Auth redirect.
        public OAuthToken GetOAuthToken(string authCode, string state)
        {
            var restClient = new RestClient(MONZO_OAUTH);

            var request = new RestRequest(Method.POST);

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", _clientId);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("redirect_uri", _redirectUri);
            request.AddParameter("code", authCode);

            var response = restClient.Execute(request);
            var content = response.Content;

            var accessResponse = JsonConvert.DeserializeObject<OAuthToken>(content);
            return accessResponse;
        }
    }
}
