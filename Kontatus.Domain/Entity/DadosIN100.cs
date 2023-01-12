
using ConsigIntegra.Domain.Enums;
using System.Collections.Generic;

namespace ConsigIntegra.Domain.Entity
{
    public class DadosIN100 : Base
    {
        public string SolicitacaoID { get; set; }
        public int? BeneficiarioID { get; set; }
        public string Cpf { get; set; }
        public string NumeroBeneficio { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public string EmprestimoBloqueado { get; set; }
        public string EmprestimoElegivel { get; set; }
        public string DIB { get; set; }
        public int? BeneficioID { get; set; }
        public string MargemConsignavel { get; set; }
        public int? RMCAtivo { get; set; }
        public string UFBeneficio { get; set; }
        public int? MeioPagamentoID { get; set; }
        public int? InstituicaoFinanceiraID { get; set; }
        public string NomeAgencia { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroContaCorrente { get; set; }
        public string DataAtualizacao { get; set; }
        public string MensagemServidor { get; set; }
        public string DDB { get; set; }
        public int? RequisicaoID { get; set; }
        public string Situacao { get; set; }
        public int? OrigemBancoID { get; set; }
        public string MargemConsignavelCartao { get; set; }
        public string QtdEmprestimosAtivosSuspensos { get; set; }
        public string PossuiRepresentanteLegal { get; set; }
        public string PossuiProcurador { get; set; }
        public int? Idade { get; set; }
        public string MeioPagamento { get; set; }
        public string Banco { get; set; }
        public string Beneficio { get; set; }
        public bool MargemNula { get; set; }

    }
}
