using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
           return Challenge(new AuthenticationProperties() { RedirectUri = "Home/Index" },
                    Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme);
        }

        public ActionResult Logout()
        {
            return SignOut(new AuthenticationProperties() { RedirectUri = "Home/Index"},
                Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme,
                Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
