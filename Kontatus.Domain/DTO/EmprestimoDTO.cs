
using ConsigIntegra.Domain.Entity;
using System.Collections.Generic;

namespace ConsigIntegra.Domain.DTO
{
    public class EmprestimoDTO
    {
        public string NumeroContratoEmprestimo { get; set; }
        public string DescricaoTipoEmprestimo { get; set; }
        public string SituacaoEmprestimo { get; set; }
        public string ValorParcelaEmprestimo { get; set; }
        public string ParcelasPagasEmprestimo { get; set; }
        public string PrazoEmprestimo { get; set; }
        public string CompetenciaInicioDescontoEmprestimo { get; set; }
        public string CompetenciaFinalDescontoEmprestimo { get; set; }
        public string ValorEmprestimo { get; set; }
        public string SaldoEmprestimo { get; set; }
        public string TaxaEmprestimo { get; set; }
        public string DataAverbacaoEmprestimo { get; set; }

    }
}
