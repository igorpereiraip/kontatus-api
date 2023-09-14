using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Pessoa")]
    public class Pessoa : Base
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string Nome { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string CPF { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string DataNascimento { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1)]
        public string Sexo { get; set; }
        public bool Aposentado { get; set; }

    }
}
