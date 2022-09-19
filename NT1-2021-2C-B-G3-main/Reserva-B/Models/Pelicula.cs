using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Pelicula
    {

        public int Id { get; set; }

        private readonly TimeSpan duracion = new TimeSpan(2, 0, 0);
        public TimeSpan Duracion { get { return duracion; } }


        //[Required(ErrorMessage = ErrorMsgs.Requerido)] // ver tema de validacion de fecha
        //[DataType(DataType.DateTime, ErrorMessage = ErrorMsgs.NoValido)] // ver 
        //[Display(Name = Alias.FechaDeLanzamiento)]

        //public Date.Now FechaDeLanzamiento { get; set; } = Date.Now;

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Date, ErrorMessage = ErrorMsgs.NoValido)]
        //[Display(Name = Alias.FechaDeLanzamiento)]
        //[Remote(action: "FechaIncorrecta", controller: "Peliculas")]
        public DateTime FechaDeLanzamiento { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Titulo { get; set; }



        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo7, MinimumLength = Restriccion.Piso4, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Descripcion { get; set; }


        [Display(Name = Alias.Genero)]
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public int GeneroId { get; set; }
        public Genero Genero { get; set; }

        public List<Funcion> Funciones { get; set; }

        public string Foto { get; set; } = "default.jpg";

        public decimal Subtotal
        {
            get
            {
                decimal resultado = 0;
                if (this.Funciones != null)
                {
                    foreach (Funcion funcion in this.Funciones)
                    {
                        resultado += funcion.Subtotal;
                    }

                }
                return resultado;
            }


        }
    }
}
