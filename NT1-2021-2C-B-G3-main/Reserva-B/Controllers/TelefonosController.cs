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
    public class TelefonosController : Controller
    {
        private readonly ReservaContext _context;
        private readonly SignInManager<Persona> signInManager;

        public TelefonosController(ReservaContext context, SignInManager<Persona> signInManager)
        {
            _context = context;
            this.signInManager = signInManager;
        }

        // GET: Telefonos
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            var reservaContext = _context.Telefonos.Include(t => t.Persona);
            return View(await reservaContext.ToListAsync());
        }

        // GET: Telefonos/Details/5
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // GET: Telefonos/Create
        [Authorize(Roles = "Cliente, Empleado")]
        public IActionResult Create()
        {
           // ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto");
            return View();
        }

        // POST: Telefonos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado, Cliente")]
        public async Task<IActionResult> Create([Bind("Id,Principal,Tipo,CodArea,Numero")] Telefono telefono)
        {
            var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);
            var telefonoContext = _context.Telefonos.Include(t => t.PersonaId).Where(t => t.PersonaId == personaLogueada.Id);
           
            if (ModelState.IsValid)
            {
                telefono.PersonaId = personaLogueada.Id;
                _context.Add(telefono);
                await _context.SaveChangesAsync();
                //return RedirectToAction("Details", "Clientes", new {id= telefono.PersonaId });

                if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Cliente")))
                {
                    return RedirectToAction("Details", "Clientes", new { id = telefono.PersonaId });

                }
                else if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Empleado")))
                {
                    return RedirectToAction("Details", "Empleados", new { id = telefono.PersonaId });
                }
            }
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", telefono.PersonaId);
            return View(telefono);
        }

        // GET: Telefonos/Edit/5
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
        
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos.FindAsync(id);
            if (telefono == null)
            {
                return NotFound();
            }
            //ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", telefono.PersonaId);
            return View(telefono);
        }

        // POST: Telefonos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Principal,Tipo,CodArea,Numero")] Telefono telefono)
        {

            var personaLogueada = await signInManager.UserManager.GetUserAsync(HttpContext.User);

            var telefonoContext = _context.Telefonos.Include(t => t.PersonaId).Where(t => t.PersonaId == personaLogueada.Id);

            if (id != telefono.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    telefono.PersonaId = personaLogueada.Id;
                    _context.Update(telefono);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelefonoExists(telefono.Id))
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
                        return RedirectToAction("Details", "Clientes", new { id = telefono.PersonaId });

                    }
                    else if ((signInManager.IsSignedIn(User)) && (User.IsInRole("Empleado")))
                    {
                        return RedirectToAction("Details", "Empleados", new { id = telefono.PersonaId });
                    }
                
               // return RedirectToAction("Details", "Clientes", new { id = telefono.PersonaId });
            }
            ViewData["PersonaId"] = new SelectList(_context.Personas, "Id", "NombreCompleto", telefono.PersonaId);
            return View(telefono);
        }

        // GET: Telefonos/Delete/5
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telefono = await _context.Telefonos
                .Include(t => t.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (telefono == null)
            {
                return NotFound();
            }

            return View(telefono);
        }

        // POST: Telefonos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente, Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var telefono = await _context.Telefonos.FindAsync(id);
            _context.Telefonos.Remove(telefono);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelefonoExists(int id)
        {
            return _context.Telefonos.Any(e => e.Id == id);
        }
    }
}
