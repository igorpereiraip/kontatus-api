
using ConsigIntegra.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ConsigIntegra.Domain.DTO
{
    public class UsuarioDTO
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Administrador { get; set; }
        public bool Ativo { get; set; }
        public int SaldoIN100 { get; set; }
        public int SaldoExtrato { get; set; }
        public int? SaldoOffline { get; set; }
        public bool? OfflineIlimitado { get; set; }
        public int? LimiteDiario { get; set; }
        public int? AcessosSimultaneos { get; set; }
        public DateTime? ValidadePlano { get; set; }

    }
}
