using Kontatus.Domain.Enums;

namespace Kontatus.Domain.Entity
{

    public class SolicitacaoIN100 : Base
    {
        public int? UsuarioID { get; set; }
        public string SolicitacaoID { get; set; }
        public string NumeroBeneficio { get; set; }
        public StatusProcessamentoEnum StatusProcessamento { get; set; }
        public bool? CreditoExtrato { get; set; }
        public virtual Usuario Usuario { get; set; }

    }

}
