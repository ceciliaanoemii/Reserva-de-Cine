using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;

namespace Reserva_B.Controllers
{
    public class ReservasClienteController : Controller
    {
        private readonly ReservaContext _context;
        private readonly SignInManager<Persona> signInManager;

        public ReservasClienteController(ReservaContext context, SignInManager<Persona> signInManager)
        {
            _context = context;
            this.signInManager = signInManager;
        }

        // GET: ReservasCliente
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Index()
        {
            var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);

            var reservaContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion).Where(r => r.ClienteId == personaLogueada.Id);

            return View(await reservaContext.ToListAsync());
        }


   

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            DateTime semana = DateTime.Today.AddDays(7);
             DateTime hoy = DateTime.Today;
            var test = _context.Funciones.Include(f => f.Sala)
                        .Include(f => f.Reservas).ToList();


            ViewData["FuncionId"] = new SelectList(_context.Funciones.Where(f => f.PeliculaId == id && f.Fecha >= hoy && f.Fecha < semana && f.Confirmada).ToArray(), "Id", "Descripcion");  
          
            return View();
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create([Bind("FechaAlta,CantidadButacas,ClienteId,FuncionId")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                //Valido todo lo que tengo que validar
                var funcion = _context.Funciones
                        .Include(f => f.Sala)
                        .Include(f => f.Reservas)
                        .FirstOrDefault(f => f.Id == reserva.FuncionId);


                if (funcion.CalcularButacasDisponibles() >= reserva.CantidadButacas)
                {

                    var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);
                    var reservasActivas = _context.Reservas.Where(r => r.ReservaActiva).Where(r => r.ClienteId == personaLogueada.Id).ToArray();
                    
                    //Creo la reserva
                    if (reservasActivas.Length == 0)
                    {
                        reserva.ClienteId = personaLogueada.Id;
                        _context.Add(reserva);
                        reserva.ReservaActiva = true;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {  
                        ModelState.AddModelError("", "No puede reservar ya que tiene una reserva activa." );
                    }
                    return View(reserva);
                }
                else
                {
                    //Anda a metroplex
                    ModelState.AddModelError("", "No hay dispobles la cantidad de butacas que quiere reservar ");
                }                               
                return View(reserva);
            }
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
           
            return View(reserva);
        }




        // GET: ReservasCliente/Details/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .ThenInclude(f => f.Sala)
                .ThenInclude(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }



        // GET: ReservasCliente/Delete/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Funcion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: ReservasCliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var reserva = _context.Reservas.Include(r => r.Funcion).FirstOrDefault(r => r.Id == id);


            if (DateTime.Now <= reserva.Funcion.Fecha.AddHours(-24))
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //agregan error
                ModelState.AddModelError("Funcion", "No se puede cancelar la reserva 24 hs antes de la funcion");
                //ME TIENE QUE APARECER EL MENSAJE DE ERROR

            }
            //vuelven a la misma vista, para ver el error.
            return View(reserva);
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
