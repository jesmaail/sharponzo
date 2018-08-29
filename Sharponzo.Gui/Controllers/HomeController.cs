using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sharponzo.OAuth;

namespace Sharponzo.Gui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Authorisation()
        {
            var auth = new Auth();
            auth.Authenticate();
            throw new NotImplementedException();
        }
    }
}
