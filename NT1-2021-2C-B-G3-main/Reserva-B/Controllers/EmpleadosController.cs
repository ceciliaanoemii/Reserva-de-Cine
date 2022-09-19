using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;

namespace Reserva_B.Controllers
{
    [Authorize]
    public class EmpleadosController : Controller
    {
        private readonly ReservaContext _context;
        private readonly UserManager<Persona> userManager;
        private readonly SignInManager<Persona> signInManager;
        private readonly RoleManager<Rol> roleManager;
        private const string passPorDefecto = "Contraseña1!";

        public EmpleadosController(ReservaContext context, UserManager<Persona> userManager, SignInManager<Persona> signInManager, RoleManager<Rol> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        // GET: Empleados
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                                                .Include(p => p.Direccion)
                                                .Include(p => p.Telefonos)
                                                .FirstOrDefaultAsync(m => m.Id == id);

            if (empleado == null) //No lo encontre en el contexto, o sea en mi base de datos
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public async Task<IActionResult> Create(bool crearDireccion, [Bind("Legajo,Id,Dni,Nombre,Apellido,UserName,Email,FechaAlta,Password")] Empleado emp1)
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Legajo,Id,Dni,Nombre,Apellido,UserName,Email,FechaAlta,Password")] Empleado emp1)
        {
            if (ModelState.IsValid)
            {

                Empleado empleado = new Empleado();
                empleado.Nombre = emp1.Nombre;
                empleado.Apellido = emp1.Apellido;
                empleado.Legajo = emp1.Legajo;
                empleado.Dni = emp1.Dni;
                empleado.Email = emp1.Email;
                empleado.UserName = emp1.Email; //VER DESPUÉS
                empleado.Password = passPorDefecto;
                try
                {
                    //Camino feliz
                    var resultadoCreacion = await userManager.CreateAsync(empleado, emp1.Password);

                    if (resultadoCreacion.Succeeded)
                    {
                        if (!await roleManager.RoleExistsAsync("Empleado"))
                        {
                            await roleManager.CreateAsync(new Rol("Empleado"));
                        }

                        var resultado = await userManager.AddToRoleAsync(empleado, "Empleado");

                        if (resultado.Succeeded)
                        {
                            await signInManager.SignInAsync(empleado, isPersistent: false);
                            return RedirectToAction("Index", "Empleados", new { id = empleado.Id });
                            // return RedirectToAction("Create", "Direcciones", new { id = empleado.Id }); saco la redireccion a crear domicilio
                        }
                    }
                   
                    //agregarn los errores


                }
                catch (DbUpdateException dbex)
                {
                    //tratamiento al problema de ducplicado
                    SqlException innerException = dbex.InnerException as SqlException;

                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601))
                    {
                        ModelState.AddModelError("Dni", "El dni ya esta registrado");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbex.Message);
                    }

                }


            }
            return View(emp1);
        }


    

        // GET: Empleados/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> LegajoExistente(string legajo)
        {
            var legajoExiste = _context.Empleados.FirstOrDefault(p => p.Legajo.Contains(legajo));

            if (legajoExiste == null)
            {
                return Json(true);  //no existe empleado con ese legajo
            }
            else
            {
                return Json($"El legajo {legajo} ya está en uso.");  //El legajo ya esta en uso
            }
        }
    }
}
