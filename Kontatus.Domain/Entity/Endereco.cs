using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Endereco")]
    public class Endereco : Base
    {
        public int PessoaId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Cidade { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Bairro { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string DescricaoEndereco { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string Complemento { get; set; }
        public virtual Pessoa Pessoa { get; set; }

    }
}
