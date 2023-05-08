﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MvcCubosExamenEnriqueGPB.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MvcCubosExamenEnriqueGPB.Filters;

namespace MvcCubosExamenEnriqueGPB.Controllers
{
    public class ManagedController : Controller
    {
        private ServiceCubos service;
        public ManagedController(ServiceCubos service)
        {
                this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            string token =
                await this.service.GetTokenAsync(email, password);
            if (token == null)
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            else
            {
                ViewData["MENSAJE"] = "Bienvendo a la web cubos";
                HttpContext.Session.SetString("token", token);
                ClaimsIdentity identity =
                    new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, password));



                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15)
                    });
                return RedirectToAction("Index", "Cubos");

            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }
    }
}
