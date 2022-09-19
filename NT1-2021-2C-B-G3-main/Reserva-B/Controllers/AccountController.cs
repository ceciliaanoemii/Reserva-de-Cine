using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reserva_B.Data;
using Reserva_B.Models;
using Reserva_B.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Controllers
{
    public class AccountController : Controller
    {
        private readonly ReservaContext _context;
        private readonly UserManager<Persona> userManager;
        private readonly SignInManager<Persona> signInManager;
        private readonly RoleManager<Rol> roleManager;
        private const string passPorDefecto = "Contraseña1!";

        public AccountController(ReservaContext context, UserManager<Persona> userManager, SignInManager<Persona> signInManager, RoleManager<Rol> roleManager)
        {
            this._context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(RegistracionViewModel registro)
        {

            if (ModelState.IsValid) {

                Cliente cliente = new Cliente();
                cliente.Nombre = registro.Nombre;
                cliente.Apellido = registro.Apellido;
                cliente.Dni = registro.Dni;
                cliente.Email = registro.Email;
                cliente.UserName = registro.Email; //VER DESPUÉS
                cliente.Password = registro.Password;
                

                var resultadoCreacion = await userManager.CreateAsync(cliente, registro.Password);

                if (resultadoCreacion.Succeeded)
                {
                    if (!roleManager.Roles.Any())
                    {
                        await CrearRoles();
                    }

                    var resultado = await userManager.AddToRoleAsync(cliente, "Cliente");

                    if (resultado.Succeeded)
                    {
                        await signInManager.SignInAsync(cliente, isPersistent: false);
                        return RedirectToAction("Cartelera", "Peliculas", new{id = cliente.Id});
                    }
                }

                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(registro);
        }

        [HttpGet]
        public ActionResult IniciarSesion(string returnurl)
        {
            TempData["returnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IniciarSesion(InicioSesion inicioSesion)
        {

            string returnUrl = TempData["returnUrl"] as string;

            if (ModelState.IsValid)
            {
                var resultadoInicioSesion = await signInManager.PasswordSignInAsync(inicioSesion.Email, inicioSesion.Password, inicioSesion.Recordarme, false);


                if (resultadoInicioSesion.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                        return RedirectToAction("Cartelera", "Peliculas");
                }

                ModelState.AddModelError(string.Empty, "No se pudo iniciar sesión");
            }

            return View(inicioSesion);
        }

        public async Task<ActionResult> CerrarSesion()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Cartelera", "Peliculas");
        }


        public async Task CrearRoles()
        {
            List<string> roles = new List<string>() { "Cliente", "Empleado" };

            if (!_context.Roles.Any())
            {
                foreach (string rol in roles)
                {
                    await CrearRol(rol);
                }
            }
        } 


        private async Task CrearRol(string rolName)
        {
            if(!await roleManager.RoleExistsAsync(rolName))
            {
                await roleManager.CreateAsync(new Rol(rolName));
            }
        }



        [HttpGet]
        public async Task<IActionResult> EmailDisponible(string email)
        {
            var emailOcupado = _context.Personas.Any(p => p.Email == email);

            if (!emailOcupado)
            {
                return Json(true);
            }
            else
            {
                return Json($"El correo {email} ya está en uso");
            }
        }

    }
}

