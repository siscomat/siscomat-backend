using System;
using System.Collections.Generic;
using System.Text;

namespace Siscomat.Core.DTOs
{
    public class LoginDTO
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
