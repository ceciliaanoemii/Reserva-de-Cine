using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;
using Microsoft.AspNetCore.Authorization;

namespace Reserva_B.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaContext _context;

        public ReservasController(ReservaContext context)
        {
            _context = context;
        }

        // GET: Reservas
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var reservaContext = _context.Reservas.Include(r => r.Cliente).Include(r => r.Funcion);
            return View(await reservaContext.ToListAsync());
        }

        // GET: Reservas/Details/5
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        //public IActionResult Create()
        //{
        //    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Apellido");
        //    ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion");
        //    return View();
        //}

        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create()
        {
            //if(peliculaId == null)
            //{
            //    return RedirectToAction("Cartelera", "Peliculas");
            //}
            var test = _context.Funciones.Include(f => f.Sala)
                        .Include(f => f.Reservas).ToList();
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto");
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion");
            return View();

        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Create([Bind("Id,FechaAlta,CantidadButacas,ClienteId,FuncionId,ReservaActiva")] Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                //Valido todo lo que tengo que validar
                var funcion = _context.Funciones
                        .Include(f => f.Sala)
                        .Include(f => f.Reservas)
                        .FirstOrDefault(f => f.Id == reserva.FuncionId);

                if (funcion.CalcularButacasDisponibles() >= reserva.CantidadButacas )
                {
                    //Creo la reserva
                    if (!reserva.ReservaActiva)
                    {
                        _context.Add(reserva);
                        reserva.ReservaActiva = true;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                       
                    
                    //else
                    //{
                    //    ModelState.AddModelError(reserva., "No puede reservar ya que tiene una reserva activa.");
                    //}
                    
                }
                else
                {
                    //Anda a metroplex
                    ModelState.AddModelError("CantidadButacas", "No hay dispobles la cantidad de butacas que quiere reservar");
                }
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaAlta,CantidadButacas,ClienteId,FuncionId")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "NombreCompleto", reserva.ClienteId);
            ViewData["FuncionId"] = new SelectList(_context.Funciones, "Id", "Descripcion", reserva.FuncionId);
            return View(reserva);
        }

        // GET: Reservas/Delete/5
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

        // POST: Reservas/Delete/5
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
