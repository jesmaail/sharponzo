using Newtonsoft.Json;
using RestSharp;
using Sharponzo.Models;

namespace Sharponzo.Auth
{
    public class Authorisation
    {
        private readonly ClientDetails _clientDetails;
        private const string MONZO_OAUTH = "https://api.monzo.com/oauth2/token";
        // This should live in config?
        private const string REDIRECT_URI = "http://localhost:5000/Home/Callback";

        public Authorisation(ClientDetails clientDetails)
        {
            _clientDetails = clientDetails;
        }

        public string GetAuthRequestUrl()
        {
            return $"https://auth.monzo.com/?client_id={_clientDetails.Id}&redirect_uri={REDIRECT_URI}&response_type=code";
        }

        public AccessResponse GetAccessCode(string authCode, string state)
        {
            var restClient = new RestClient(MONZO_OAUTH);

            var request = new RestRequest(Method.POST);

            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("client_id", _clientDetails.Id);
            request.AddParameter("client_secret", _clientDetails.Secret);
            request.AddParameter("redirect_uri", REDIRECT_URI);
            request.AddParameter("code", authCode);

            var response = restClient.Execute(request);
            var content = response.Content;

            var accessResponse = JsonConvert.DeserializeObject<AccessResponse>(content);
            return accessResponse;
        }
    }
}
