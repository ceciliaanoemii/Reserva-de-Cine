using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reserva_B.Helpers
{
    public class ErrorMsgs
    {
        public const string Requerido = "El campo {0} es requerido";
        public const string RangoMinMax = "El campo {0} debe estar comprendido entre {1} y {2}";
        public const string StrMaxMin = "El campo {0}, debe tener un mínimo de {2} y un máximo de {1}";
        public const string StrMin = "El campo {0}, debe tener un minimo de {1}";
        public const string Generico = "Verifique el ingreso del campo {0}";
        public const string YaExiste = "El campo {0} ya existe en la base de datos";
        public const string NoValido = "El campo {0} no es válido";
        public const string FixedSize = "El campo {0} debe tener {1} caracteres";
        public const string PasswordError = "El campo {0} no coincide";
    }
}
