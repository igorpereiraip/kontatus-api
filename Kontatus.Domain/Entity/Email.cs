using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Email")]
    public class Email : Base
    {
        public int PessoaId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        public string EnderecoEmail { get; set; }
        public int ArquivoImportadoId { get; set; }
        public virtual ArquivoImportado ArquivoImportado { get; set; }
        //public virtual Pessoa Pessoa { get; set; }

    }
}
