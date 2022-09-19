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
    public class DireccionesController : Controller
    {
        private readonly ReservaContext _context;
        private readonly SignInManager<Persona> signInManager;
        public DireccionesController(ReservaContext context, SignInManager<Persona> signInManager)
        {
            _context = context;
            this.signInManager = signInManager;
        }

        // GET: Direcciones
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var reservaContext = _context.Direcciones.Include(d => d.Persona);
            return View(await reservaContext.ToListAsync());
        }

        // GET: Direcciones/Details/5
        [Authorize(Roles = "Empleado, Cliente")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // GET: Direcciones/Create
        [Authorize(Roles = "Cliente, Empleado")]
        public IActionResult Create(int? id)
        {
                                                          
            return View();
        }

        // POST: Direcciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Calle,CodPostal,Numero,Piso,Departamento")] Direccion direccion)
        {
            var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);

            var direccionContext = _context.Direcciones.Include(d => d.PersonaId).Where(d => d.PersonaId == personaLogueada.Id);
           
            if (ModelState.IsValid)
            {
                direccion.PersonaId = personaLogueada.Id;
                _context.Add(direccion);
               await _context.SaveChangesAsync();

                if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Cliente")))
                {
                    return RedirectToAction("Details", "Clientes", new { id = direccion.PersonaId });

                }
                else if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Empleado")))
                {
                    return RedirectToAction("Details", "Empleados", new { id = direccion.PersonaId });
                }
            }
            
            return View(direccion);
        }

        // GET: Direcciones/Edit/5
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones.FindAsync(id);
            if (direccion == null)
            {
                return NotFound();
            }
            
            return View(direccion);
        }

        // POST: Direcciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Calle,CodPostal,Numero,Piso,Departamento")] Direccion direccion)
        {
            var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);

            var direccionContext = _context.Direcciones.Include(d => d.PersonaId).Where(d => d.PersonaId == personaLogueada.Id);

            if (id != direccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    direccion.PersonaId = personaLogueada.Id;
                    _context.Update(direccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DireccionExists(direccion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Cliente")))
                {
                    return RedirectToAction("Details", "Clientes", new { id = direccion.PersonaId });

                } else if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Empleado")))
                {
                    return RedirectToAction("Details", "Empleados", new { id = direccion.PersonaId });
                }

                    //return RedirectToAction("Details", "Clientes", new { id = direccion.PersonaId });
            }
          
            return View(direccion);
        }

        // GET: Direcciones/Delete/5
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var direccion = await _context.Direcciones
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (direccion == null)
            {
                return NotFound();
            }

            return View(direccion);
        }

        // POST: Direcciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var direccion = await _context.Direcciones.FindAsync(id);
            _context.Direcciones.Remove(direccion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DireccionExists(int id)
        {
            return _context.Direcciones.Any(e => e.Id == id);
        }
    }
}
