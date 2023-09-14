using Kontatus.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("ArquivoImportado")]
    public class ArquivoImportado : Base
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Competencia { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Descricao { get; set; }
        public StatusProcessamentoEnum StatusProcessamento { get; set; }
        public int? PessoasAdicionadas { get; set; }
        public int? EnderecosCriados { get; set; }
        public int? TelefonesCriados { get; set; }
        public int? EmailsCriados { get; set; }
    }
}
