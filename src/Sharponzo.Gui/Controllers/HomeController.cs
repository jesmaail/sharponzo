using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sharponzo.Models;
using Sharponzo.Auth;
using Sharponzo.Logic;

namespace Sharponzo.Gui.Controllers
{
    public class HomeController : Controller
    {
        private readonly Authorisation _auth;
        private readonly ClientDetails _clientDetails;

        public HomeController(IOptions<ClientDetails> clientDetails)
        {
            _clientDetails = clientDetails.Value ?? throw new ArgumentException(nameof(clientDetails));
            _auth = new Authorisation(_clientDetails);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authorisation()
        {
            var redirectUrl = _auth.GetAuthRequestUrl();
            return Redirect(redirectUrl);
        }

        public IActionResult Callback(string code, string state)
        {
            var accessResponse = _auth.GetAccessCode(code, state);

            //  Temporary, want this in it's own view (probably get rid of Callback as well for it)
            //      - This has essentially replaced the main class in the old project now. Will need a change
            var monzoApi = new MonzoApi(accessResponse.AccessToken);
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
