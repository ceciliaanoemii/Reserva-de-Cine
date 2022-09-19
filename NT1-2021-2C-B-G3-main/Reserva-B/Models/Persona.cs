using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Reserva_B.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Reserva_B.Models
{
    public class Persona : IdentityUser<int>
    {
       // public int Id { get; set;}

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.DNI)]
        [Range(Restriccion.PisoDni, Restriccion.TechoDni, ErrorMessage = ErrorMsgs.RangoMinMax)]
        [Remote(action: "DniExistente", controller: "Clientes")]
        public int Dni { get; set; }

        

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Apellido { get; set; }

        //[Required(ErrorMessage = ErrorMsgs.Requerido)]
        //[StringLength(Restriccion.Techo5, MinimumLength = Restriccion.Piso2, ErrorMessage = ErrorMsgs.StrMaxMin)]
        //[Display(Name = Alias.UserName)]
        //public String UserName { get; set; }


        public List<Telefono> Telefonos { get; set; }

        public Direccion Direccion { get; set; }

        //[Required(ErrorMessage = ErrorMsgs.Requerido)]
        //[Display(Name = Alias.Email)]
        //[DataType(DataType.EmailAddress, ErrorMessage = ErrorMsgs.NoValido)]
        //public String Email { get; set; }

       [Required(ErrorMessage = ErrorMsgs.Requerido)]
       [Display(Name = Alias.FechaAlta)]
       [DataType(DataType.DateTime, ErrorMessage = ErrorMsgs.NoValido)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

       [Required(ErrorMessage = ErrorMsgs.Requerido)]
       [Display(Name = Alias.Password)]
       [DataType(DataType.Password)]
       [StringLength(Restriccion.Techo3, MinimumLength = Restriccion.Piso3, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Password { get; set; }

        [Display(Name = Alias.NombreCompleto)]
        public string NombreCompleto { get
            {
                return $" {Apellido.ToUpper()}, {Nombre}";
            }
        }



    }
}
