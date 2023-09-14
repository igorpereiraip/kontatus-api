using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Usuario")]
    public class Usuario : Base
    {
        [StringLength(250, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Nome do Usuario é Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Flag Administrador é Obrigatório")]
        public bool Administrador { get; set; }


        public virtual ICollection<Login> Logins { get; set; }

        public int? AcessosSimultaneos { get; set; }
        public DateTime? ValidadePlano { get; set; }
    }
}
