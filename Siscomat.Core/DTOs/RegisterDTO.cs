using System;
using System.Collections.Generic;
using System.Text;

namespace Siscomat.Core.DTOs
{
    public class RegisterDTO
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string Apellido2 { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
