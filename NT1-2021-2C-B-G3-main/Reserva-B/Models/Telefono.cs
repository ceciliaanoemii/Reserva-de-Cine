using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Reserva_B.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reserva_B.Models
{
    public class Telefono
    {

        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.CodArea)]
        [Range(Restriccion.Piso1, Restriccion.Techo11, ErrorMessage = ErrorMsgs.RangoMinMax)]
        public int CodArea { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.PhoneNumber)]
        public int Numero { get; set; }

        public bool Principal { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public TipoTelefono Tipo { get; set; }



        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public int PersonaId { get; set; }

        public Persona Persona { get; set; }

        [NotMapped]
        public string NumberoCompleto { get { return $"({CodArea}) - {Numero}"; } }
        
    }
}
