using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Direccion
    {
        //[Key, ForeignKey("Persona")]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]

        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]

        [Display(Name = Alias.Calle)]

        public String Calle { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Range(Restriccion.Piso7, Restriccion.Techo9, ErrorMessage = ErrorMsgs.RangoMinMax)]
        [Display(Name = Alias.CodPost)]
        public int CodPostal { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Range(Restriccion.Piso1, Restriccion.Techo9, ErrorMessage = ErrorMsgs.RangoMinMax)]
        public int Numero { get; set; }


        [Range(Restriccion.Piso0, Restriccion.Techo4, ErrorMessage = ErrorMsgs.RangoMinMax)]
        public Nullable<int> Piso { get; set; }


        [StringLength(Restriccion.Techo1, MinimumLength = Restriccion.Piso0, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Departamento { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.Persona)]
        public int PersonaId { get; set; }

        public Persona Persona { get; set; }

        [NotMapped]
        public String DireccionCompleta { get { return $"{Calle}  {Numero} - Codigo Postal: {CodPostal} - Piso: {Piso} - Depto: {Departamento}"; } }


    }
}
