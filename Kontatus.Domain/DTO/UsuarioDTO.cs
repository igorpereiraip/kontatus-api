
using Kontatus.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Kontatus.Domain.DTO
{
    public class UsuarioDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Administrador { get; set; }
        public bool Ativo { get; set; }
        public int? AcessosSimultaneos { get; set; }
        public DateTime? ValidadePlano { get; set; }

    }
}
