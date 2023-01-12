using System;
using System.Collections.Generic;

namespace Kontatus.Domain.ViewModels
{
    public class TokenConfigViewModel
    {
        public string JWT { get; set; }
        public DateTime Expiration { get; set; }
        public LoginViewModel Login { get; set; }
        public UsuarioViewModel Usuario { get; set; }
    }
}
