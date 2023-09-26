using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Telefone")]
    public class Telefone : Base
    {
        public int PessoaId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string NumeroTelefone { get; set; }     
        public bool Whatsapp { get; set; }
        public int ArquivoImportadoId { get; set; }
        public virtual ArquivoImportado ArquivoImportado { get; set; }
        //public virtual Pessoa Pessoa { get; set; }

    }

}
