using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Sala
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Range(Restriccion.Piso1, Restriccion.Techo3, ErrorMessage = ErrorMsgs.RangoMinMax)]
        [Remote(action: "SalaExistente", controller: "Salas")]
        [Display(Name = Alias.Numero)]
        public int Numero { get; set; }

        public TipoSala TipoSala { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.CapacidadButacas)]
        [Range(Restriccion.Piso5, Restriccion.Techo6, ErrorMessage = ErrorMsgs.RangoMinMax)]
        public int CapacidadButacas { get; set; }


        public List<Funcion> Funciones { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.TipoSala)]
        public int TipoSalaId { get; set; }

    }
}
