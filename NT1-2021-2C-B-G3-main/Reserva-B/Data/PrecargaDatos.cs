using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reserva_B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Data
{
    public class PrecargaDatos : IDbInit
    {

        private readonly UserManager<Persona> userManager;
        private readonly RoleManager<Rol> rolManager;
        private readonly ReservaContext contexto;
        private const string passPorDefecto = "Password1!";

        public PrecargaDatos(UserManager<Persona> userManager, RoleManager<Rol> rolManager, ReservaContext contexto)
        {
            this.userManager = userManager;
            this.rolManager = rolManager;
            this.contexto = contexto;
        }

        public void Seed()
        {
            CrearRoles().Wait();
            CrearDireccion().Wait();
            CrearTelefono().Wait();
            CrearEmpleado().Wait();
            CrearCliente().Wait();
            CrearGeneros().Wait();
            CrearTipoSalas().Wait();
            CrearSalas().Wait();
            CrearPeliculas().Wait();  
            CrearFunciones().Wait();
            CrearReservas().Wait();
        }               
      

        public async Task CrearRoles()
        {
            List<string> roles = new List<string>() { "Cliente", "Empleado" };

            if (!contexto.Roles.Any())
            {
                foreach (string rol in roles)
                {
                    await CrearRol(rol);
                }
            }
        }


        private async Task CrearRol(string rolName)
        {
            if (!await rolManager.RoleExistsAsync(rolName))
            {
                await rolManager.CreateAsync(new Rol(rolName));
            }
        }

        private async Task CrearEmpleado()
        {

            var empleado = contexto.Personas.Any(p => p.Email.Equals("empleado@empleado.com"));

            if (!empleado)
            {
                Empleado empleado1 = new Empleado()
                {
                    Legajo = "33",
                    Dni = 36541896,
                    Nombre = "Empleado",
                    Apellido = "Prueba",
                    Email = "empleado@empleado.com",
                    UserName = "empleado@empleado.com"
                };

                var resultadoCreacion = await userManager.CreateAsync(empleado1, passPorDefecto);

                if (resultadoCreacion.Succeeded)
                {                   
                    await CrearRol("Empleado");
                    await userManager.AddToRoleAsync(empleado1, "Empleado");
                }

            }
        }

        public async Task CrearCliente()
        {
            var cliente = contexto.Personas.Any(p => p.Email.Equals("cliente@cliente.com"));

            if (!cliente)
            {
                Cliente cliente1 = new Cliente()
                {
                    Dni = 33698521,
                    Nombre = "Cliente",
                    Apellido = "Cliente",
                    Email = "cliente@cliente.com",
                    UserName = "cliente@cliente.com",
                    Direccion = contexto.Direcciones.FirstOrDefault(d => d.Numero == 1698),                  
                };
               
                var resultadoCreacion = await userManager.CreateAsync(cliente1, passPorDefecto);

                if (resultadoCreacion.Succeeded)
                {
                    await CrearRol("Cliente");
                    await userManager.AddToRoleAsync(cliente1, "Cliente");
                }

                Cliente cliente2 = new Cliente()
                {
                    Dni = 42589632,
                    Nombre = "Cliente1",
                    Apellido = "Cliente1",
                    Email = "cliente1@cliente.com",
                    UserName = "cliente1@cliente.com",
                    Direccion = contexto.Direcciones.FirstOrDefault(d => d.Numero == 1698),
                };

                var resultadoCreacion1 = await userManager.CreateAsync(cliente2, passPorDefecto);

                if (resultadoCreacion1.Succeeded)
                {
                    await CrearRol("Cliente");
                    await userManager.AddToRoleAsync(cliente2, "Cliente");
                }

                Cliente cliente3 = new Cliente()
                {
                    Dni = 37896541,
                    Nombre = "Cliente2",
                    Apellido = "Cliente2",
                    Email = "cliente2@cliente.com",
                    UserName = "cliente2@cliente.com",
                    Direccion = contexto.Direcciones.FirstOrDefault(d => d.Numero == 1698),
                };

                var resultadoCreacion2 = await userManager.CreateAsync(cliente3, passPorDefecto);

                if (resultadoCreacion2.Succeeded)
                {
                    await CrearRol("Cliente");
                    await userManager.AddToRoleAsync(cliente1, "Cliente");
                }

            }

        }


        public async Task CrearDireccion()
        {
            Direccion direccion = new Direccion()
            {
                Calle = "Cordoba",
                CodPostal = 1425,
                Numero = 1698,
                Piso = 1,
                Departamento = "A",
            };
        }

         public async Task CrearTelefono()
         {
                Telefono telefono = new Telefono()
                {
                    CodArea = 054,
                    Numero = 1146986352,
                    Principal = true,
                    Tipo = TipoTelefono.Celular,
                    Persona = contexto.Personas.FirstOrDefault(p => p.Dni == 33698521),
                };
         }

        private async Task CrearPeliculas()
        {

            if (!contexto.Peliculas.Any())
            {
                Pelicula pelicula = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 12),
                    Titulo = "¿Qué paso ayer?",
                    Descripcion = "Un grupo de amigos no recuerda nada de lo sucedido la noche anterior",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Comedia")),
                    Foto = "QuePasoAyer.jpg",
                };
                Pelicula pelicula1 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 9),
                    Titulo = "Titanic",
                    Descripcion = "Se hunde un crucero a causa de un iceberg",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Drama")),
                    Foto = "Titanic.jpg",

                };
                Pelicula pelicula2 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 14),
                    Titulo = "Anabelle",
                    Descripcion = "Una muñeca maldita aterroriza a una familia",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Terror")),
                    Foto = "Anabelle.jpg",

                };
                Pelicula pelicula3 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 20),
                    Titulo = "Eternals",
                    Descripcion = "Un grupo de superhéroes busca salvar al planeta de una amenaza mortal.",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Ciencia Ficción")),
                    Foto = "Eternals.jpg",

                };
                Pelicula pelicula4 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 05),
                    Titulo = "Dune",
                    Descripcion = "El viaje de un héroe mítico y cargado de emociones",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Ciencia Ficción")),
                    Foto = "Dune.jpg",

                };
                Pelicula pelicula5 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 10),
                    Titulo = "Actividad Paranormal",
                    Descripcion = "Una familia empieza a vivir sucesos extraños en su departamento",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Terror")),
                    Foto = "ActividadParanormal.png",

                };
                Pelicula pelicula6 = new Pelicula
                {
                    FechaDeLanzamiento = new DateTime(2021, 11, 13),
                    Titulo = "Una noche en el museo",
                    Descripcion = "Un museo parece no tener nada fuera de lo normla hasta que llega la noche",
                    Genero = contexto.Generos.FirstOrDefault(g => g.Nombre.Equals("Comedia")),
                    Foto = "NocheEnElMuseo.jpg",

                };


                await contexto.Peliculas.AddAsync(pelicula);
                await contexto.Peliculas.AddAsync(pelicula1);
                await contexto.Peliculas.AddAsync(pelicula2);
                await contexto.Peliculas.AddAsync(pelicula3);
                await contexto.Peliculas.AddAsync(pelicula4);
                await contexto.Peliculas.AddAsync(pelicula5);
                await contexto.Peliculas.AddAsync(pelicula6);

                await contexto.SaveChangesAsync();
            }
        }

        private async Task CrearGeneros()
        {
            if (!contexto.Generos.Any())
            {
                Genero genero = new Genero
                {
                    Nombre = "Drama",
                };
                Genero genero1 = new Genero
                {
                    Nombre = "Terror",
                };
                Genero genero2 = new Genero
                {
                    Nombre = "Comedia",
                };
                Genero genero3 = new Genero
                {
                    Nombre = "Ciencia Ficción",
                };

                await contexto.Generos.AddAsync(genero);
                await contexto.Generos.AddAsync(genero1);
                await contexto.Generos.AddAsync(genero2);
                await contexto.Generos.AddAsync(genero3);

                await contexto.SaveChangesAsync();
            }
        }

        private async Task CrearTipoSalas()
        {
            if (!contexto.TipoDeSalas.Any())
            {
                TipoSala tipo = new TipoSala
                {
                    Nombre = "Estandar",
                    Precio = 500,
                    
                };
                TipoSala tipo1 = new TipoSala
                {
                    Nombre = "3D",
                    Precio = 700,
                    
                };
                TipoSala tipo2 = new TipoSala
                {
                    Nombre = "Premium",
                    Precio = 1000,
                    
                };

                await contexto.TipoDeSalas.AddAsync(tipo);
                await contexto.TipoDeSalas.AddAsync(tipo1);
                await contexto.TipoDeSalas.AddAsync(tipo2);

                await contexto.SaveChangesAsync();
            }
        }


        private async Task CrearSalas()
        {
            if (!contexto.Salas.Any())
            {
                Sala sala = new Sala
                {
                    Numero = 1,
                    CapacidadButacas = 70,
                    TipoSala = contexto.TipoDeSalas.FirstOrDefault(t => t.Nombre.Equals("Premium")),
                    // TIENE UNA LISTA FUNCIONES
                };
                Sala sala1 = new Sala
                {
                    Numero = 5,
                    CapacidadButacas = 100,
                    TipoSala = contexto.TipoDeSalas.FirstOrDefault(g => g.Nombre.Equals("Estandar")),
                    // TIENE UNA LISTA FUNCIONES
                };
                Sala sala2 = new Sala
                {
                    Numero = 8,
                    CapacidadButacas = 85,
                    TipoSala = contexto.TipoDeSalas.FirstOrDefault(g => g.Nombre.Equals("3D")),
                    // TIENE UNA LISTA FUNCIONES
                };

                await contexto.Salas.AddAsync(sala);
                await contexto.Salas.AddAsync(sala1);
                await contexto.Salas.AddAsync(sala2);

                await contexto.SaveChangesAsync();
            }
        }

        private async Task CrearFunciones()
        {
            if (!contexto.Funciones.Any())
            {
                Funcion funcion = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 19),
                    Hora = new DateTime(2021, 11, 19, 15, 30, 00),
                    Descripcion = "Titanic, el día 19/11/2021 a las 15:30hs",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Titanic")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 1),
                    
                };
                Funcion funcion1 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 22),
                    Hora = new DateTime(2021, 11, 22, 20, 00, 00),
                    Descripcion = "Titanic, el día 22/11/2021 a las 20:00hs",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Titanic")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 5),
                    
                };
                Funcion funcion2 = new Funcion
                {
                    Fecha = new DateTime(2021, 11, 23),
                    Hora = new DateTime(2021, 11, 23, 16, 15, 00),
                    Descripcion = "Anabelle, el día 23/11/2021 a las 16:15",
                    Confirmada = false,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Anabelle")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };
                Funcion funcion3 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 19),
                    Hora = new DateTime(2021, 11, 19, 23, 45, 00),
                    Descripcion = "Anabelle, el día 19/11/2021 a las 23:45",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Anabelle")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 1),
                   
                };
                Funcion funcion4 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 09),
                    Hora = new DateTime(2021, 11, 09, 13, 00, 00),
                    Descripcion = "¿Qué paso ayer?, el día 01/12/2021 a las 13:00",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("¿Qué paso ayer?")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };


                Funcion funcion5 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 18),
                    Hora = new DateTime(2021, 11, 18, 18, 00, 00),
                    Descripcion = "¿Qué paso ayer?, el día 18/11/2021 a las 18:00",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("¿Qué paso ayer?")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 5),
                    
                };

                Funcion funcion6 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 17),
                    Hora = new DateTime(2021, 11, 17, 19, 30, 00),
                    Descripcion = "Eternals, el día 17/11/2021 a las 19:30",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Eternals")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 5),
                    
                };

                Funcion funcion7 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 17),
                    Hora = new DateTime(2021, 11, 17, 23, 30, 00),
                    Descripcion = "Eternals, el día 17/11/2021 a las 23:30",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Eternals")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };

                Funcion funcion8 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 16),
                    Hora = new DateTime(2021, 11, 16, 14, 00, 00),
                    Descripcion = "Dune, el día 16/11/2022 a las 14:00",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Dune")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };

                Funcion funcion9 = new Funcion
                {

                    Fecha = new DateTime(2022, 01, 01),
                    Hora = new DateTime(2022, 01, 01, 19, 00, 00),
                    Descripcion = "Dune, el día 01/01/2022 a las 19:00",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Dune")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 1),
                    
                };

                Funcion funcion10 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 20),
                    Hora = new DateTime(2021, 11, 20, 16, 40, 00),
                    Descripcion = "Actividad Paranormal, el día 20/11/2021 a las 16:40",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Actividad Paranormal")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 1),
                    
                };

                Funcion funcion11 = new Funcion
                {

                    Fecha = new DateTime(2021, 12, 01),
                    Hora = new DateTime(2021, 12, 01, 21, 00, 00),
                    Descripcion = "Actividad Paranormal, el día 01/12/2021 a las 21:00",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Actividad Paranormal")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };

                Funcion funcion12 = new Funcion
                {

                    Fecha = new DateTime(2021, 12, 08),
                    Hora = new DateTime(2021, 12, 08, 23, 30, 00),
                    Descripcion = "Una noche en el museo, el día 08/12/2021 a las 23:30",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Una noche en el museo")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };

                Funcion funcion13 = new Funcion
                {

                    Fecha = new DateTime(2021, 11, 19),
                    Hora = new DateTime(2021, 11, 19, 18, 30, 00),
                    Descripcion = "Una noche en el museo, el día 19/11/2021 a las 18:30",
                    Confirmada = true,
                    Pelicula = contexto.Peliculas.FirstOrDefault(p => p.Titulo.Equals("Una noche en el museo")),
                    Sala = contexto.Salas.FirstOrDefault(s => s.Numero == 8),
                    
                };

                await contexto.Funciones.AddAsync(funcion);
                await contexto.Funciones.AddAsync(funcion1);
                await contexto.Funciones.AddAsync(funcion2);
                await contexto.Funciones.AddAsync(funcion3);
                await contexto.Funciones.AddAsync(funcion4);
                await contexto.Funciones.AddAsync(funcion5);
                await contexto.Funciones.AddAsync(funcion6);
                await contexto.Funciones.AddAsync(funcion7);
                await contexto.Funciones.AddAsync(funcion8);
                await contexto.Funciones.AddAsync(funcion9);
                await contexto.Funciones.AddAsync(funcion10);
                await contexto.Funciones.AddAsync(funcion11);
                await contexto.Funciones.AddAsync(funcion12);
                await contexto.Funciones.AddAsync(funcion13);

                await contexto.SaveChangesAsync();
            }
        }

        private async Task CrearReservas()
        {
            if (!contexto.Reservas.Any())
            {
                Reserva reserva = new Reserva
                {
                    FechaAlta = new DateTime(2021, 11, 10),
                    CantidadButacas = 3,
                    Cliente = contexto.Clientes.FirstOrDefault(c => c.Dni == 33698521),
                    Funcion = contexto.Funciones.FirstOrDefault(f => f.Id == 3),

                };
                Reserva reserva1 = new Reserva
                {
                    FechaAlta = new DateTime(2021, 11, 10),
                    CantidadButacas = 3,
                    Cliente = contexto.Clientes.FirstOrDefault(c => c.Dni == 42589632),
                    Funcion = contexto.Funciones.FirstOrDefault(f => f.Id == 3),

                };
                Reserva reserva2 = new Reserva
                {
                    FechaAlta = new DateTime(2021, 11, 10),
                    CantidadButacas = 3,
                    Cliente = contexto.Clientes.FirstOrDefault(c => c.Dni == 37896541),
                    Funcion = contexto.Funciones.FirstOrDefault(f => f.Id == 3),

                };

                await contexto.Reservas.AddAsync(reserva);
                await contexto.Reservas.AddAsync(reserva1);
                await contexto.Reservas.AddAsync(reserva2);

                await contexto.SaveChangesAsync();
            }
        }
    }
}


