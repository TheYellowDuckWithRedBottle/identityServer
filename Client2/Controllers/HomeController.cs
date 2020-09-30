using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Client2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;

namespace Client2.Controllers
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
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            ViewData["idToken"] = idToken;

            var disCoveryClient = new DiscoveryClient("http://localhost:5000");
            var doc = await disCoveryClient.GetAsync();
            var userInfo = new UserInfoClient(doc.UserInfoEndpoint);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfo.GetAsync(accessToken);
            var claims = response.Claims;

            var email = claims.FirstOrDefault(x => x.Type == "email")?.Value;

            var accessToken1= await HttpContext.GetTokenAsync("access_token");
            var refresh_token= await HttpContext.GetTokenAsync("refresh_token"); 

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
