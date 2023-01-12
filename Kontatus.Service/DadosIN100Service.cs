using AutoMapper;
using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface IDadosIN100Service
    {
        Task<DadosIN100> GetByIdSolicitacao(string idSolicitacao);
    }

    public class DadosIN100Service : IDadosIN100Service
    {
        private readonly IDadosIN100Repository repository;
        private readonly IMapper _mapper;

        public DadosIN100Service(IDadosIN100Repository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

        public async Task<DadosIN100> GetByIdSolicitacao(string idSolicitacao)
        {
            var dados = await repository.GetByIdSolicitacao(idSolicitacao);
            dados = this.TransformaResultado(dados);
            return dados;
        }

        private DadosIN100 TransformaResultado(DadosIN100 dados)
        {

            if (dados.MensagemServidor.Contains("A requisição está sem o número do cpf do representante legal"))
            {
                dados.PossuiRepresentanteLegal = "SIM";
                dados.PossuiProcurador = null;
                dados.EmprestimoBloqueado = null;
                dados.EmprestimoElegivel = null;
                dados.MargemConsignavel = null;
                dados.MargemConsignavelCartao = null;
                dados.QtdEmprestimosAtivosSuspensos = null;
            }

            if (dados.MensagemServidor.Contains("estão bloqueados para empréstimo e"))
            {
                dados.PossuiRepresentanteLegal = null;
                dados.PossuiProcurador = null;
                dados.EmprestimoBloqueado = "SIM";
                dados.EmprestimoElegivel = null;
                dados.MargemConsignavel = null;
                dados.MargemConsignavelCartao = null;
                dados.QtdEmprestimosAtivosSuspensos = null;
            }

            return dados;

        }

    }
}
