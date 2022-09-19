using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Empleado : Persona
    {
       
        [Required(ErrorMessage = ErrorMsgs.Requerido)]

        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]

        [Remote (action: "LegajoExistente", controller: "Empleados")]
        
        public String Legajo { get; set; }

       
    }

}
