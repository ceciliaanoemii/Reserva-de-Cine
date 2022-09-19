using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;

namespace Reserva_B.Controllers
{
    public class FuncionesController : Controller
    {
        private readonly ReservaContext _context;

        public FuncionesController(ReservaContext context)
        {
            _context = context;
        }

        // GET: Funciones
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var reservaContext = _context.Funciones.Include(f => f.Pelicula).Include(f => f.Sala).Include(f => f.Reservas);
            return View(await reservaContext.ToListAsync());
        }

        // GET: Funciones/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // GET: Funciones/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo");
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id");
            return View();
        }

        // POST: Funciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Hora,Descripcion,Confirmada,PeliculaId,SalaId")] Funcion funcion)
        {
             if (ModelState.IsValid)
            {                                          
                    _context.Add(funcion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                
            }
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id", funcion.SalaId);

            return View(funcion);
        }

        // GET: Funciones/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id", funcion.SalaId);
            return View(funcion);
        }

        // POST: Funciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Hora,Descripcion,ButacasDisponibles,Confirmada,PeliculaId,SalaId")] Funcion funcion)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
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
            ViewData["PeliculaId"] = new SelectList(_context.Peliculas, "Id", "Titulo", funcion.PeliculaId);
            ViewData["SalaId"] = new SelectList(_context.Salas, "Id", "Id", funcion.SalaId);
            return View(funcion);
        }

        // GET: Funciones/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .Include(f => f.Reservas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // POST: Funciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcion = await _context.Funciones
            .Include(f => f.Pelicula)
            .Include(f => f.Sala)
            .Include(f => f.Reservas)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (!funcion.Reservas.Any())
                {
                    _context.Funciones.Remove(funcion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "No se puede cancelar la función ya que cuenta con reservas.");
                }

                return View(funcion);
        }

        private bool FuncionExists(int id)
        {
            return _context.Funciones.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> FechaIncorrecta(DateTime Fecha)
        {
            if(Fecha > DateTime.Today)
            {
                return Json(true);
            }
            else
            {
                return Json("La fecha ingresada es incorrecta");
            }         

        }

        public async Task<IActionResult> RevisarFunciones()
        {
            var funciones = _context.Funciones.Include(f => f.Reservas);

            foreach (Funcion funcion in funciones)
            {
                if (funcion.Fecha <= DateTime.Today)
                {
                    var reservas = funcion.Reservas;
                    foreach(Reserva reserva in reservas)
                    {
                        if (reserva.ReservaActiva == true)
                        {
                            reserva.ReservaActiva = false;
                            _context.Update(reserva);                          
                        }                  
                    }                                     
                }             
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Funciones");
        }

        //    public async Task<IActionResult> MostrarFuncionesDeLaSemana(int id)
        //    {
        //        DateTime semana = DateTime.Today.AddDays(7);
        //        DateTime hoy = DateTime.Today;

        //        var funciones = _context.Funciones.Where(f => f.Fecha >= hoy && f.Fecha < semana && f.Confirmada);
        //        return View(funciones);
        //    }

    }
    
}
