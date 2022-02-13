using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiCompleto.Extensions;
using MvcPeliculasApiCompleto.Filters;
using MvcPeliculasApiCompleto.Models;
using MvcPeliculasApiCompleto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Controllers
{
    public class ClientesController : Controller
    {
        private ServiceApiPeliculas service;

        public ClientesController(ServiceApiPeliculas service)
        {
            this.service = service;
        }

        [AuthorizeClientes]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.User.FindFirst("TOKEN").Value;
            Cliente cliente = await this.service.GetPerfilCliente(token);
            return View(cliente);
        }

        [AuthorizeClientes]
        public async Task<IActionResult> Pedidos()
        {
            string token =
                HttpContext.User.FindFirst("TOKEN").Value;
            List<PedidosCliente> pedidos =
                await this.service.GetPeidosCliente(token);
            return View(pedidos);
        }

        [AuthorizeClientes]
        public async Task<IActionResult> FinalizarPedido()
        {
            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Pelicula> peliculas =
                    await this.service.GetCarritoPeliculasAsync(carrito);
            string datacliente =
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string token =
                HttpContext.User.FindFirst("TOKEN").Value;
            int idcliente = int.Parse(datacliente);
            foreach (Pelicula peli in peliculas)
            {
                await this.service.AddPedido(idcliente, peli.IdPelicula
                    , 1, DateTime.Now, peli.Precio, token);
            }
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("Pedidos", "Clientes");
        }
    }
}
