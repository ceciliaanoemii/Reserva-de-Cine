using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Genero
    {
        public int Id { get; set; }



        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
       //[Remote(action: "GeneroExistente", controller: "Generos")]
        public String Nombre { get; set; }


        public List<Pelicula> Peliculas { get; set; }

     
    }
}
