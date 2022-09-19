using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Data;
using Reserva_B.Models;
using Reserva_B.ViewModels;

namespace Reserva_B.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ReservaContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PeliculasController(ReservaContext context,IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Cartelera(int ? idGenero)
        {
            var peliculas = _context.Peliculas.Include(p => p.Genero).ToList();

            if (idGenero != null)
            {
              
                peliculas = peliculas.Where(p => p.GeneroId == idGenero).ToList();

            }
            return View(peliculas);
        }

        // GET: Peliculas
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Peliculas.ToListAsync());
        }

        // GET: Peliculas/Details/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas    
               .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SubirFoto(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TempData["PeliculaId"] = id.Value;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> SubirFoto(FotoPelicula fotovm)
        {
            int idPelicula = (int)TempData["PeliculaId"];
            if (idPelicula == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string nombreDeArchivo = Guid.NewGuid().ToString() + "_" + fotovm.Imagen.FileName;
                string carpeta = Path.Combine(_hostingEnvironment.WebRootPath, "img\\");
                string pathFinal = Path.Combine(carpeta, nombreDeArchivo);

                Pelicula pelicula = _context.Peliculas.Find(idPelicula);
                fotovm.Imagen.CopyTo(new FileStream(pathFinal, FileMode.Create));

                pelicula.Foto = nombreDeArchivo;

                _context.Update(pelicula);
                _context.SaveChanges();

            }


            return View();
        }


        [Authorize(Roles = "Empleado")]
        public IActionResult Create()
        {
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");


            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Create([Bind("Id,FechaDeLanzamiento,Titulo,Descripcion,GeneroId")] Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }




        // GET: Peliculas/Edit/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaDeLanzamiento,Titulo,Descripcion")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
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
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }

        //[HttpGet]
        //public async Task<IActionResult> FechaIncorrecta (DateTime FechaDeLanzamiento)
        //{
        //    if (FechaDeLanzamiento > DateTime.Today)
        //    {
        //        return Json(true);
        //    }
        //    else
        //    {
        //        return Json("La fecha ingresada es incorrecta");
        //    }

        //}

    }
}


