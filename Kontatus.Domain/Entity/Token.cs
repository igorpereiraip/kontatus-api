using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Token")]
    public class Token : Base
    {
        public string JWT { get; set; }
        public DateTime DataExpiracao { get; set; }
        public int UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
