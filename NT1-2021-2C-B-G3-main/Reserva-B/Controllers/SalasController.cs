using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;

namespace Reserva_B.Controllers
{
    public class SalasController : Controller
    {
        private readonly ReservaContext _context;

        public SalasController(ReservaContext context)
        {
            _context = context;
        }

        // GET: Salas
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var reservaContext = _context.Salas.Include(s => s.TipoSala).ToList();
            return View(reservaContext);
        }

        // GET: Salas/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas
                .Include(s => s.TipoSala)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null)
            {
                return NotFound();
            }

            return View(sala);
        }

        // GET: Salas/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["TipoSalaId"] = new SelectList(_context.TipoDeSalas, "Id", "Nombre");
            return View();
        }

        // POST: Salas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Id,Numero,CapacidadButacas,TipoSalaId")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(sala);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {                         
                       ModelState.AddModelError("Numero", "El número de sala ya existe");                                                  
                }

                ViewData["TipoSalaId"] = new SelectList(_context.TipoDeSalas, "Id", "Nombre", sala.TipoSalaId);            
            }
            return View(sala);
        }

        // GET: Salas/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sala = await _context.Salas.FindAsync(id);
            if (sala == null)
            {
                return NotFound();
            }
            ViewData["TipoSalaId"] = new SelectList(_context.TipoDeSalas, "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numero,CapacidadButacas,TipoSalaId")] Sala sala)
        {
            if (id != sala.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaExists(sala.Id))
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
            ViewData["TipoSalaId"] = new SelectList(_context.TipoDeSalas, "Id", "Nombre", sala.TipoSalaId);
            return View(sala);
        }

        
        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> SalaExistente(int numero)
        {
            var salaExiste = _context.Salas.FirstOrDefault(p => p.Numero == numero);

            if (salaExiste == null)
            {
                return Json(true);  
            }
            else
            {
                return Json($"El número de sala {numero} ya existe");
            }
        }
    }


}
