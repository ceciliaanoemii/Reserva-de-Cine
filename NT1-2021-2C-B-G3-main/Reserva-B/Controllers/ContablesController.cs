using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Controllers
{
    public class ContablesController : Controller
    {
        private readonly ReservaContext _context;

        public ContablesController(ReservaContext context)
        {
            _context = context;
        }

        

        [HttpGet]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> BalanceMensual()
        {
            var ahora = DateTime.Now;
            var pisoFecha = new DateTime(ahora.Year, ahora.Month, 1);
           

            var datos =  _context.Funciones
                                 .Include(f=>f.Sala).ThenInclude(s=>s.TipoSala)
                                 .Include(f => f.Reservas)
                                 .Include(f=> f.Pelicula)
                                 .Where(f => f.Fecha >= pisoFecha && f.Fecha <= ahora)
                                 .ToList();


            decimal total = 0;
            
            foreach(Funcion funcion in datos)
            {
                total += funcion.Subtotal;
            }
                     

            return View(datos);
        }
    }
}
