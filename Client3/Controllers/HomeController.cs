using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;

namespace Client3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            ViewData["idToken"] = idToken;

            var disCoveryClient = new DiscoveryClient("http://localhost:5000");
            var doc = await disCoveryClient.GetAsync();
            var userInfo = new UserInfoClient(doc.UserInfoEndpoint);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfo.GetAsync(accessToken);
            var claims = response.Claims;
            var email = claims.FirstOrDefault(x => x.Type == "email")?.Value;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
