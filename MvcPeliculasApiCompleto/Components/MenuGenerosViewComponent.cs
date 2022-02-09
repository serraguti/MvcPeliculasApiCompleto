using Microsoft.AspNetCore.Mvc;
using MvcPeliculasApiCompleto.Models;
using MvcPeliculasApiCompleto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Components
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private ServiceApiPeliculas service;

        public MenuGenerosViewComponent(ServiceApiPeliculas service)
        {
            this.service = service;
        }

        //TODO COMPONENT TIENE UN METODO PARA DEVOLVER EL DIBUJO AL LAYOUT
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.service.GetGenerosAsync();
            return View(generos);
        }
    }
}
