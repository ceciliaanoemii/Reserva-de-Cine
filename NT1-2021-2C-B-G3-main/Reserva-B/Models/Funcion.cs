using Microsoft.AspNetCore.Mvc;
using Reserva_B.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Models
{
    public class Funcion
    {

        public int Id { get; set; }


        private DateTime _fecha;

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Date, ErrorMessage = ErrorMsgs.NoValido)]
        [Remote(action: "FechaIncorrecta", controller: "Funciones")]
        public DateTime Fecha
        {
            get { return _fecha; }
            set
            {
                var fechaCompleta = new DateTime(value.Year, value.Month, value.Day, _hora.Hour, _hora.Minute, _hora.Second);
                _fecha = fechaCompleta;
                _hora = fechaCompleta;
            }
        }


        private DateTime _hora;

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Time, ErrorMessage = ErrorMsgs.NoValido)]
        public DateTime Hora
        {
            get { return _hora; }
            set
            {
                var fechaCompleta = new DateTime(Fecha.Date.Year, Fecha.Date.Month, Fecha.Date.Day, value.Hour, value.Minute, value.Second);
                _fecha = fechaCompleta;
                _hora = fechaCompleta;
            }
        }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(Restriccion.Techo7, MinimumLength = Restriccion.Piso4, ErrorMessage = ErrorMsgs.StrMaxMin)]
        public String Descripcion { get; set; }


        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = Alias.ButacasDisponibles)]
        [NotMapped]
        public int ButacasDisponibles
        {
            get
            {
                if (Sala != null && Reservas != null)
                {
                    return CalcularButacasDisponibles();
                }

                return 0;
            }
        }

        public int CalcularButacasDisponibles()
        {
            int acumulador = Sala.CapacidadButacas;

            var reservasRealizadas = Reservas.Where(r => r.FuncionId == Id);

            if (reservasRealizadas.Any())
            {
                foreach (Reserva r in reservasRealizadas)
                {
                    acumulador -= r.CantidadButacas;
                }
            }
            return acumulador;
        }


        public Boolean Confirmada { get; set; }

        public Pelicula Pelicula { get; set; }

        public Sala Sala { get; set; }

        public List<Reserva> Reservas { get; set; }


        [Display(Name = Alias.Pelicula)]
        public int PeliculaId { get; set; }

        [Display(Name = Alias.Sala)]
        public int SalaId { get; set; }

        public decimal Subtotal
        {
            get
            {
                decimal resultado = 0;
                if (this.Reservas != null && this.Sala != null && this.Sala.TipoSala != null)
                {
                    foreach(Reserva reserva in this.Reservas)
                    {
                        resultado += reserva.Subtotal;
                    }

                }
                return resultado;
            }
        }

    }
    
}
