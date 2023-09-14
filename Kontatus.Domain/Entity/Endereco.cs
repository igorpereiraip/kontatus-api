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
        [StringLength(100)]
        public string DescricaoEndereco { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string Complemento { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(10)]
        public string Cep { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(2)]
        public string Uf { get; set; }
        public int ArquivoImportadoId { get; set; }
        public virtual ArquivoImportado ArquivoImportado { get; set; }
        public virtual Pessoa Pessoa { get; set; }

    }
}
