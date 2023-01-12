using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ConsigIntegra.Domain.Entity
{
    public class ResultadoIN100
    {
        public int? BeneficiarioID { get; set; }
        public string Cpf { get; set; }
        public string NumeroBeneficio { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool? EmprestimoBloqueado { get; set; }
        public bool? EmprestimoElegivel { get; set; }
        public DateTime? DIB { get; set; }
        public int? BeneficioID { get; set; }
        public float? MargemConsignavel { get; set; }
        public int? RMCAtivo { get; set; }
        public string UFBeneficio { get; set; }
        public int? MeioPagamentoID { get; set; }
        public int? InstituicaoFinanceiraID { get; set; }
        public string NomeAgencia { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroContaCorrente { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string MensagemServidor { get; set; }
        public DateTime? DDB { get; set; }
        public int? RequisicaoID { get; set; }
        public string Situacao { get; set; }
        public int? OrigemBancoID { get; set; }
        public float? MargemConsignavelCartao { get; set; }
        public float? QtdEmprestimosAtivosSuspensos { get; set; }
        public bool? PossuiRepresentanteLegal { get; set; }
        public bool? PossuiProcurador { get; set; }
        public int? Idade { get; set; }

    }
}
