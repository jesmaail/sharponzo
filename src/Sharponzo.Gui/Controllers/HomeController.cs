using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sharponzo.Models;
using Sharponzo.Logic;

namespace Sharponzo.Gui.Controllers
{
    public class HomeController : Controller
    {
        private readonly SharponzoAuth _auth;
        private readonly ClientDetails _clientDetails;

        private const string REDIRECT_URI = "http://localhost:5000/Home/Callback";

        public HomeController(IOptions<ClientDetails> clientDetails)
        {
            _clientDetails = clientDetails.Value ?? throw new ArgumentException(nameof(clientDetails));
            _auth = new SharponzoAuth(_clientDetails.Id, _clientDetails.Secret, REDIRECT_URI);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authorisation()
        {
            var redirectUrl = _auth.GetAuthRedirectUrl();
            return Redirect(redirectUrl);
        }

        public IActionResult Callback(string code, string state)
        {
            var accessResponse = _auth.GetOAuthToken(code, state);

            //  Temporary, want this in it's own view (probably get rid of Callback as well for it)
            //      - This has essentially replaced the main class in the old project now. Will need a change
            var monzoApi = new SharponzoClient(accessResponse.AccessToken);
            var accountList = monzoApi.GetAccountList();
            var primaryAccountId = accountList[0].Id; // Just getting the first for now
            var account = monzoApi.GetAccount(primaryAccountId);

            var balance = account.Balance.FormattedBalance;

            var filter = new Filter(account.Payments);
            var tescoPayments = filter.GetPaymentsByMerchant("tesco");


            return View("Index");
        }
    }
}
