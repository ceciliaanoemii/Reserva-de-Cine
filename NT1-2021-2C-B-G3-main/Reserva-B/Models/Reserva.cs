using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Reserva
    { 
        public int Id { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Date, ErrorMessage = ErrorMsgs.NoValido)]
        [Display(Name = Alias.FechaAlta)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;
        

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Range (Restriccion.Piso1, Restriccion.Techo6, ErrorMessage =ErrorMsgs.RangoMinMax)]
        [Display(Name = Alias.CantidadButacas)]
        public int CantidadButacas { get; set; }


        [Display(Name = Alias.Cliente)]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }


        [Display(Name = Alias.Funcion)]
        public int FuncionId { get; set; }
        public Funcion Funcion { get; set; }

        public Boolean ReservaActiva { get; set; } = false;

        [NotMapped]
        public decimal Subtotal
        {
            get {
                decimal resultado = 0;
                if(this.Funcion != null && this.Funcion.Sala !=null && this.Funcion.Sala.TipoSala != null)
                {
                    resultado = CantidadButacas * Funcion.Sala.TipoSala.Precio;
                    
                }
                return resultado;
            }
        }
    }

}
