using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Kontatus.Domain.Entity
{
    public class ResultadoOffline
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public int? Idade { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public string NomeMae { get; set; }
        public string Banco { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroContaCorrente { get; set; }
        public string MeioPagamento { get; set; }
        public string NumeroBeneficio { get; set; }
        public int? CodigoTipoBeneficio { get; set; }
        public string DescricaoTipoBeneficio { get; set; }
        public string SituacaoBeneficio { get; set; }
        public bool? EmprestimoBloqueado { get; set; }
        public string DescricaoEmprestimoBloqueado { get; set; }
        public bool? RepresentanteProcurador { get; set; }
        public string DescricaoRepresentanteProcurador { get; set; }
        public bool? PossuiRepresentante { get; set; }
        public string DescricaoPossuiRepresentante { get; set; }
        public string CpfRepresentanteLegal { get; set; }
        public string NomeRepresentanteLegal { get; set; }
        public bool? PossuiProcurador { get; set; }
        public string DescricaoPossuiProcurador { get; set; }
        public bool? PensaoAlimenticia { get; set; }
        public string DescricaoPensaoAlimenticia { get; set; }
        public bool? PermiteEmprestimo { get; set; }
        public string DescricaoPermiteEmprestimo { get; set; }
        public DateTime? DIB { get; set; }
        public DateTime? DDB { get; set; }
        public string Competencia { get; set; }
        public string UFBeneficio { get; set; }
        public float? SalarioBruto { get; set; }
        public float? SalarioBase { get; set; }
        public float? MargemEmprestimo { get; set; }
        public float? MargemCartao { get; set; }
        public float? MargemCartaoBeneficio { get; set; }
        public float? MargemAumentoSalarial { get; set; }
        public DateTime? DataAverbacao { get; set; }
        public string BancoEmprestimo { get; set; }
        public string NumeroContrato { get; set; }
        public string DescricaoTipoEmprestimo { get; set; }
        public string SituacaoCartao { get; set; }
        public float? LimiteCartao { get; set; }
        public float? ValorReservadoCartao { get; set; }
        public string TipoConsignacao { get; set; }
        public string SituacaoEmprestimo { get; set; }
        public float? ValorParcela { get; set; }
        public float? ParcelasPagas { get; set; }
        public float? Prazo { get; set; }
        public string CompetenciaInicioDesconto { get; set; }
        public string CompetenciaFinalDesconto { get; set; }
        public float? ValorEmprestimo { get; set; }
        public float? Saldo { get; set; }
        public float? Taxa { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Telefone3 { get; set; }
        public string Telefone4 { get; set; }
        public string Telefone5 { get; set; }
        public string Telefone6 { get; set; }
        public string Telefone7 { get; set; }
        public string Telefone8 { get; set; }
        public string Telefone9 { get; set; }
        public string Telefone10 { get; set; }
        public string Telefone11 { get; set; }
        public string Telefone12 { get; set; }
        public string Telefone13 { get; set; }
        public string Telefone14 { get; set; }
        public string Telefone15 { get; set; }
        public string WhatsApp1 { get; set; }
        public string WhatsApp2 { get; set; }
        public string WhatsApp3 { get; set; }
        public string WhatsApp4 { get; set; }
        public string WhatsApp5 { get; set; }
        public string WhatsApp6 { get; set; }
        public string WhatsApp7 { get; set; }
        public string WhatsApp8 { get; set; }
        public string WhatsApp9 { get; set; }
        public string WhatsApp10 { get; set; }
        public string WhatsApp11 { get; set; }
        public string WhatsApp12 { get; set; }
        public string WhatsApp13 { get; set; }
        public string WhatsApp14 { get; set; }
        public string WhatsApp15 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
