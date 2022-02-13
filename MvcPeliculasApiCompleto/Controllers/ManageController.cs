using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiCompleto.Models;
using MvcPeliculasApiCompleto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Controllers
{
    public class ManageController : Controller
    {
        private ServiceApiPeliculas service;

        public ManageController(ServiceApiPeliculas service)
        {
            this.service = service;
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> 
            LogIn(String email, string password)
        {
            string token =
                await this.service.GetTokenAsync(email, password);
            if (token != null)
            {
                Cliente cliente
                    = await this.service.GetPerfilCliente(token);
                ClaimsIdentity identity = new ClaimsIdentity
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier
                    , cliente.IdCliente.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, cliente.Nombre));
                //GUARDAMOS TAMBIEN EL TOEKN DEL CLIENTE
                identity.AddClaim(new Claim("TOKEN", token));
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, principal
                    , new AuthenticationProperties
                    {
                        IsPersistent = true
                    ,
                        ExpiresUtc = DateTime.Now.AddMinutes(30)
                    });
                return RedirectToAction("Perfil", "Clientes");
            }
            else
            {
                ViewBag.Mensaje = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    }
}
