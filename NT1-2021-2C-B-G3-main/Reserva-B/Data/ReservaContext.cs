using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reserva_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reserva_B.ViewModels;

namespace Reserva_B.Data
{
    public class ReservaContext:IdentityDbContext <IdentityUser<int>, IdentityRole<int>, int>
    {

        public ReservaContext (DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Definición del nombre de las tablas

            modelBuilder.Entity<IdentityUser<int>>().ToTable("Personas");
            modelBuilder.Entity<Sala>().ToTable("Salas");
            modelBuilder.Entity<Persona>()
                        .HasIndex(p => p.Dni)
                        .IsUnique(true);
            modelBuilder.Entity<Sala>()
                        .HasIndex(s => s.Numero)
                        .IsUnique(true);
            modelBuilder.Entity<Genero>()
                        .HasIndex(s => s.Nombre)
                        .IsUnique(true);
            modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("PersonasRoles");
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Telefono> Telefonos { get; set; }
        public DbSet<TipoSala> TipoDeSalas { get; set; }
        


    }
}
