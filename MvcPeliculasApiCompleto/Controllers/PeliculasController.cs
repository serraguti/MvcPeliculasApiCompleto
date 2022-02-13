using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiCompleto.Extensions;
using MvcPeliculasApiCompleto.Models;
using MvcPeliculasApiCompleto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Controllers
{
    public class PeliculasController : Controller
    {
        private ServiceApiPeliculas service;

        public PeliculasController(ServiceApiPeliculas service)
        {
            this.service = service;
        }

        public async Task<IActionResult> PeliculasGenero(int id)
        {
            List<Pelicula> peliculas =
                await this.service.GetPeliculasGeneroAsync(id);
            return View(peliculas);
        }

        public async Task<IActionResult> DetailsPelicula(int idpelicula)
        {
            Pelicula peli = await this.service.FindPeliculaAsync(idpelicula);
            return View(peli);
        }

        public IActionResult AddPeliculaCarrito(int idpelicula)
        {
            List<int> idspeliculas;
            if (HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
            {
                idspeliculas = new List<int>();
            }else
            {
                idspeliculas =
                    HttpContext.Session.GetObject<List<int>>("CARRITO");
            }
            idspeliculas.Add(idpelicula);
            HttpContext.Session.SetObject<List<int>>("CARRITO", idspeliculas);
            return RedirectToAction("CarritoCompra", "Peliculas");
        }

        public async Task<IActionResult> CarritoCompra(int? ideliminar)
        {
            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito == null)
            {
                ViewData["MENSAJE"] = "No hay peliculas en el carrito";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    carrito.Remove(ideliminar.Value);
                    if (carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                        return View();
                    }
                    else
                    {
                        HttpContext.Session.SetObject<List<int>>("CARRITO", carrito);
                    }
                }
                List<Pelicula> peliculas =
                    await this.service.GetCarritoPeliculasAsync(carrito);
                return View(peliculas);
            }
        }
    }
}
