using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class TipoSala
    {

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
        [Display(Name = Alias.TipoSala)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Range(Restriccion.Piso6, Restriccion.Techo8, ErrorMessage = ErrorMsgs.RangoMinMax)]
        public decimal Precio { get; set; } 


        public List<Sala> Salas { get; set; }
    }
}
