using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Cliente : Persona 

    {
        public List<Reserva> Reservas { get; set; }


      
    }
}
