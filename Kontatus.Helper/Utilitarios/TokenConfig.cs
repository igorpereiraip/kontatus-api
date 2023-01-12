using ConsigIntegra.Domain.Entity;
using System;
using System.Collections.Generic;

namespace ConsigIntegra.Helper.Utilitarios
{
    public class TokenConfig
    {
        public string JWT { get; set; }
        public DateTime Expiration { get; set; }
        public Login Login { get; set; }
        public Usuario Usuario { get; set; }
    }
}
