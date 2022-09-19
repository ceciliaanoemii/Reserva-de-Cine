using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.ViewModels
{
    public class RegistracionViewModel

    {
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

    
            [Required(ErrorMessage = ErrorMsgs.Requerido)]
            [EmailAddress(ErrorMessage = ErrorMsgs.NoValido)]
            [Display(Name = Alias.Email)]
            [Remote(action: "EmailDisponible", controller: "Account")]
            public string Email { get; set; }


            [Required(ErrorMessage = ErrorMsgs.Requerido)]
            [DataType(DataType.Password, ErrorMessage = ErrorMsgs.NoValido)]
            [Display(Name = Alias.Password)]
            public string Password { get; set; }


            [Required(ErrorMessage = ErrorMsgs.Requerido)]
            [DataType(DataType.Password, ErrorMessage = ErrorMsgs.NoValido)]
            [Compare("Password", ErrorMessage = ErrorMsgs.PasswordError)]
            [Display(Name = Alias.PasswordConfirm)]
            public string ConfirmacionPassword { get; set; }

    }
}




