using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sharponzo.Models;
using Sharponzo.Auth;

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

            return View("Index");
        }
    }
}
