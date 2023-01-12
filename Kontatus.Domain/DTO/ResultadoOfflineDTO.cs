using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ConsigIntegra.Domain.DTO
{
    public class ResultadoOfflineDTO
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public int? Idade { get; set; }
        public string RG { get; set; }
        public string Sexo { get; set; }
        public string NomeMae { get; set; }
        public string Banco { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroContaCorrente { get; set; }
        public string MeioPagamento { get; set; }
        public string NumeroBeneficio { get; set; }
        public string CodigoTipoBeneficio { get; set; }
        public string DescricaoTipoBeneficio { get; set; }
        public string SituacaoBeneficio { get; set; }
        public string EmprestimoBloqueado { get; set; }
        public string DescricaoEmprestimoBloqueado { get; set; }
        public string RepresentanteProcurador { get; set; }
        public string DescricaoRepresentanteProcurador { get; set; }
        public string PossuiRepresentante { get; set; }
        public string DescricaoPossuiRepresentante { get; set; }
        public string CpfRepresentanteLegal { get; set; }
        public string NomeRepresentanteLegal { get; set; }
        public string PossuiProcurador { get; set; }
        public string DescricaoPossuiProcurador { get; set; }
        public string PensaoAlimenticia { get; set; }
        public string DescricaoPensaoAlimenticia { get; set; }
        public string PermiteEmprestimo { get; set; }
        public string DescricaoPermiteEmprestimo { get; set; }
        public string DIB { get; set; }
        public string DDB { get; set; }
        public string Competencia { get; set; }
        public string UFBeneficio { get; set; }
        public string SalarioBruto { get; set; }
        public string SalarioBase { get; set; }
        public string MargemEmprestimo { get; set; }
        public string MargemCartao { get; set; }
        public string MargemCartaoBeneficio { get; set; }
        public string MargemAumentoSalarial { get; set; }
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

        public List<BancoEmprestimoDTO> BancosEmprestimos { get; set; }

        public string BancoCartao1 { get; set; }
        public string NumeroContratoCartao1 { get; set; }
        public string SituacaoCartao1 { get; set; }
        public string LimiteCartao1 { get; set; }
        public string ValorReservadoCartao1 { get; set; }
        public string TipoConsignacaoCartao1 { get; set; }
        public string DataAverbacaoCartao1 { get; set; }


        public string BancoCartao2 { get; set; }
        public string NumeroContratoCartao2 { get; set; }
        public string SituacaoCartao2 { get; set; }
        public string LimiteCartao2 { get; set; }
        public string ValorReservadoCartao2 { get; set; }
        public string TipoConsignacaoCartao2 { get; set; }
        public string DataAverbacaoCartao2 { get; set; }

        public string MargemCartaoRMC { get; set; }
        public string LimiteCartaoRMC { get; set; }
        public string ValorLiberadoCartaoRMC { get; set; }
        public bool? PossivelMargemRMC { get; set; }


        public string MargemCartaoRCC { get; set; }
        public string LimiteCartaoRCC { get; set; }
        public string ValorLiberadoCartaoRCC { get; set; }
        public bool? PossivelMargemRCC { get; set; }
        public bool? RegraDataAverbacaoRCC { get; set; }



        public float? ValorMargemEmprestimo { get; set; }
        public float? ValorMargemAumentoSalarial { get; set; }


        public List<string> ListaTelefones { get; set; }
        public List<string> ListaWhatsapps { get; set; }
        public List<string> ListaEmails { get; set; }
        public string DataAtualizacao { get; set; }
    }
}
