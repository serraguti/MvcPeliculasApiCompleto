using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcPeliculasApiCompleto.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public String Nombre { get; set; }
        public string Email { get; set; }
        public string PaginaWeb { get; set; }
        public string Imagen { get; set; }
    }
}
