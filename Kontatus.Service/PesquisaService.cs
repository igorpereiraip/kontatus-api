//using Kontatus.Data.Repository;
//using Kontatus.Domain.DTO;
//using Kontatus.Domain.Entity;
//using Kontatus.Domain.Enums;
//using Kontatus.Helper.Utilitarios;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Kontatus.Service
//{
//    public interface IPesquisaService
//    {
//        Task<PesquisaBeneficio> GetDadosBeneficiosByCpfAsync(string cpf);
//        Task<PesquisaBeneficio> GetDadosBeneficiosByBeneficioAsync(string beneficio);
//        Task<(byte[], string)> ArquivoExtrato2(string beneficio);
//        Task<(byte[], string)> ArquivoExtrato1(string beneficio);
//        Task<int> SolicitarIN100Async(string cpf, string beneficio);
//        Task<int> SolicitacaoIN100Async(string cpf, string beneficio, int usuarioID, bool creditoExtrato);
//        Task<PesquisaIN100> ObterIN100Async(string numeroRequisicao);
//        Task<PesquisaIN100> ObterIN100UsuarioAsync(string numeroRequisicao);
//        Task<bool> ObterIN100Robo();
//        Task<ResultadoOfflineDTO> PesquisaExtratoOffline(string beneficio, int usuarioID);
//        Task<bool> VerificaPesquisaBeneficioDiaria(string beneficio, int usuarioID);
//    }

//    public class PesquisaService : IPesquisaService
//    {
//        private readonly ILoginRepository _loginRepository;
//        private readonly ISolicitacaoIN100Repository _solicitacaoIN100Repository;
//        private readonly ISolicitacaoOfflineRepository _solicitacaoOfflineRepository;
//        private readonly IDadosIN100Repository _dadosIN100Repository;
//        private readonly IUsuarioService _usuarioService;
//        private readonly LoginDomainService _loginDomainService;
//        private IConfiguration Configuration { get; set; }

//        public PesquisaService(ILoginRepository loginRepository,
//            LoginDomainService loginDomainService,
//            IConfiguration configuration,
//            ISolicitacaoIN100Repository solicitacaoIN100Repository,
//            IUsuarioService usuarioService,
//            IDadosIN100Repository dadosIN100Repository,
//            ISolicitacaoOfflineRepository solicitacaoOfflineRepository)
//        {
//            _loginRepository = loginRepository;
//            _loginDomainService = loginDomainService;
//            _solicitacaoIN100Repository = solicitacaoIN100Repository;
//            _dadosIN100Repository = dadosIN100Repository;
//            _usuarioService = usuarioService;
//            _solicitacaoOfflineRepository = solicitacaoOfflineRepository;
//            Configuration = configuration;
//        }

//        public async Task<PesquisaBeneficio> GetDadosBeneficiosByCpfAsync(string cpf)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/extrato/offline?pCpf={0}", cpf);
//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var beneficios = new PesquisaBeneficio();

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    beneficios = System.Text.Json.JsonSerializer.Deserialize<PesquisaBeneficio>(result);
//                }
//                else
//                    throw new Exception("Não foram encontrados benefícios para este CPF");

//                if (beneficios == null || beneficios.Beneficios == null || beneficios.Beneficios.Count == 0)
//                    throw new Exception("Não foram encontrados benefícios para este CPF");
//                else
//                    return beneficios;
//            }
//        }
//        public async Task<bool> VerificaPesquisaBeneficioDiaria(string beneficio, int usuarioID)
//        {
//            var pesquisasBeneficioDiaria = await _solicitacaoOfflineRepository.Find(x => x.Ativo && x.NumeroBeneficio == beneficio && x.UsuarioID == usuarioID &&
//                                                                                    (x.DataCadastro.Day == DateTime.Now.Day && x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year)).ToListAsync();
//            return pesquisasBeneficioDiaria.Count <= 1;
//        }

//         public async Task<ResultadoOfflineDTO> PesquisaExtratoOffline(string beneficio, int usuarioID)
//        {
//            var usuario = await _usuarioService.GetById(usuarioID);
//            var numeroChamadasOffline = _solicitacaoOfflineRepository.Find(x => x.Ativo && x.UsuarioID == usuarioID && (x.DataCadastro > DateTime.Now.AddDays(-1))).Count();

//            if((bool)usuario.OfflineIlimitado == true && numeroChamadasOffline > (int)usuario.LimiteDiario)
//                throw new Exception("Consulta OFFLINE: Trava ATIVADA por motivo de segurança. Entre em contato com o seu comercial.");


//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/extrato/offline?pNumeroBeneficio={0}", beneficio);
//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var pesquisaOffline = new PesquisaOffline();

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    pesquisaOffline = System.Text.Json.JsonSerializer.Deserialize<PesquisaOffline>(result);
//                }
//                else
//                    throw new Exception("Houve um erro ao solicitar o Extrato Offline");

//                if (pesquisaOffline == null || pesquisaOffline.Beneficios == null || pesquisaOffline.Beneficios.Count == 0 || pesquisaOffline.Resultado == null || pesquisaOffline.Resultado.Count == 0)
//                    throw new Exception("Houve um erro ao solicitar o Extrato Offline");
//                else
//                {
//                    var resultado = this.TransformaResultadoOffline(pesquisaOffline.Resultado.ToList());
//                    var solicitacaoOffline = new SolicitacaoOffline()
//                    {
//                       CPF = resultado.Cpf,
//                       UsuarioID = usuarioID,
//                       NumeroBeneficio = beneficio
//                    };
//                    await _solicitacaoOfflineRepository.Create(solicitacaoOffline);
//                    return resultado;
//                }

//            }
//        }

//        public async Task<int> SolicitarIN100Async(string cpf, string beneficio)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/IN100/solicitar?pCpf={0}&pNumeroBeneficio={1}", cpf, beneficio);
//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var numeroSolicitacao = 0;

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    numeroSolicitacao = System.Text.Json.JsonSerializer.Deserialize<int>(result);
//                }
//                else
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");

//                if (numeroSolicitacao == 0)
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");
//                else
//                    return numeroSolicitacao;
//            }
//        }

//        public async Task<int> SolicitacaoIN100Async(string cpf, string beneficio, int usuarioID, bool creditoExtrato)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/IN100/solicitar?pCpf={0}&pNumeroBeneficio={1}", cpf, beneficio);
//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var numeroSolicitacao = 0;

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    numeroSolicitacao = System.Text.Json.JsonSerializer.Deserialize<int>(result);
//                }
//                else
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");

//                if (numeroSolicitacao == 0)
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");
//                else
//                {
//                    var solicitacaoIN100 = new SolicitacaoIN100()
//                    {
//                        NumeroBeneficio = beneficio,
//                        StatusProcessamento = StatusProcessamentoEnum.EmProcessamento,
//                        SolicitacaoID = numeroSolicitacao.ToString(),
//                        UsuarioID = usuarioID,
//                        CreditoExtrato = creditoExtrato,
//                    };
//                    await _solicitacaoIN100Repository.Create(solicitacaoIN100);
//                    return numeroSolicitacao;
//                }

//            }
//        }

//        public async Task<PesquisaIN100> ObterIN100Async(string numeroRequisicao)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/IN100/obter?pNumeroRequisicao={0}", numeroRequisicao);
//            using (var client = new HttpClient())
//            {
//                var trava = true;
//                var resultadoIN100 = new PesquisaIN100();
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                while (trava)
//                {

//                    var response = await client.GetAsync(url);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        var result = response.Content.ReadAsStringAsync().Result;
//                        resultadoIN100 = System.Text.Json.JsonSerializer.Deserialize<PesquisaIN100>(result);

//                        if (resultadoIN100.Situacao == "ProcessadoComSucesso")
//                        {
//                            trava = false;
//                        }
//                        else
//                            Thread.Sleep(10000);

//                    }
//                    else
//                        throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");

//                }
//                return resultadoIN100;

//            }
//        }

//        public async Task<PesquisaIN100> ObterIN100UsuarioAsync(string numeroRequisicao)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/IN100/obter?pNumeroRequisicao={0}", numeroRequisicao);
//            using (var client = new HttpClient())
//            {
//                var trava = true;
//                var resultadoIN100 = new PesquisaIN100();
//                client.DefaultRequestHeaders.Add("apikey", apikey);

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    resultadoIN100 = System.Text.Json.JsonSerializer.Deserialize<PesquisaIN100>(result);
//                    if (resultadoIN100.Situacao == "ProcessadoComSucesso")
//                    {
//                        return resultadoIN100;
//                    }
//                    else
//                    {
//                        throw new Exception("A consulta ainda não foi concluída, favor aguardar.");
//                    }

//                }
//                else
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");


//            }
//        }

//        public async Task<PesquisaBeneficio> GetDadosBeneficiosByBeneficioAsync(string beneficio)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/extrato/offline?pNumeroBeneficio={0}", beneficio);
//            using (var client = new HttpClient())
//            {
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var beneficios = new PesquisaBeneficio();

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsStringAsync().Result;
//                    beneficios = System.Text.Json.JsonSerializer.Deserialize<PesquisaBeneficio>(result);
//                }
//                else
//                    throw new Exception("Benefício não encontrado");

//                if (beneficios == null || beneficios.Beneficios == null || beneficios.Beneficios.Count == 0)
//                    throw new Exception("Benefício não encontrado");
//                else
//                    return beneficios;
//            }
//        }

//        public async Task<(byte[], string)> ArquivoExtrato2(string beneficio)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/extrato/pdf?pNumeroBeneficio={0}&pVersao=2", beneficio);
//            using (var client = new HttpClient())
//            {
//                byte[] archiveFile;
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var beneficios = new PesquisaBeneficio();

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsByteArrayAsync().Result;
//                    archiveFile = result;
//                }
//                else
//                    throw new Exception("Benefício não encontrado");

//                if (archiveFile.Length == 0)
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");
//                else
//                    return (archiveFile, $"{beneficio}-{DateTime.Now.ToString()}.pdf");
//            }

//        }

//        public async Task<(byte[], string)> ArquivoExtrato1(string beneficio)
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var url = string.Format("https://consigtech.com/api/inss/extrato/pdf?pNumeroBeneficio={0}&pVersao=1", beneficio);
//            using (var client = new HttpClient())
//            {
//                byte[] archiveFile;
//                client.DefaultRequestHeaders.Add("apikey", apikey);
//                var beneficios = new PesquisaBeneficio();

//                var response = await client.GetAsync(url);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = response.Content.ReadAsByteArrayAsync().Result;
//                    archiveFile = result;
//                }
//                else
//                    throw new Exception("Benefício não encontrado");

//                if (archiveFile.Length == 0)
//                    throw new Exception("Erro na consulta - Favor tentar novamente. Se o problema persistir entre em contato com seu revendedor.");
//                else
//                    return (archiveFile, $"{beneficio}-{DateTime.Now.ToString()}.pdf");
//            }

//        }

//        public async Task<bool> ObterIN100Robo()
//        {
//            var consigTech = Configuration.GetSection("ConsigTech");
//            var apikey = consigTech["apiKey"];
//            var solicitacoes = await _solicitacaoIN100Repository.Find(x => x.Ativo && x.StatusProcessamento == StatusProcessamentoEnum.EmProcessamento).ToListAsync();

//            foreach (var sol in solicitacoes)
//            {
//                var url = string.Format("https://consigtech.com/api/inss/IN100/obter?pNumeroRequisicao={0}", sol.SolicitacaoID);
//                using (var client = new HttpClient())
//                {

//                    var resultadoIN100 = new PesquisaIN100();
//                    client.DefaultRequestHeaders.Add("apikey", apikey);

//                    var response = await client.GetAsync(url);
//                    if (response.IsSuccessStatusCode)
//                    {
//                        var result = response.Content.ReadAsStringAsync().Result;
//                        resultadoIN100 = System.Text.Json.JsonSerializer.Deserialize<PesquisaIN100>(result);
//                        if (resultadoIN100.Situacao == "ProcessadoComSucesso")
//                        {
//                            var resultado = this.TransformaResultado(resultadoIN100.Resultado);
//                            resultado.SolicitacaoID = sol.SolicitacaoID;
//                            await _dadosIN100Repository.Create(resultado);
//                            sol.StatusProcessamento = StatusProcessamentoEnum.ProcessadoComSucesso;
//                            await _solicitacaoIN100Repository.Update(sol);
//                        }

//                    }
//                    else
//                    {
//                        var solicitacao = await _solicitacaoIN100Repository.Find(x => x.SolicitacaoID == sol.SolicitacaoID).FirstOrDefaultAsync();
//                        solicitacao.StatusProcessamento = StatusProcessamentoEnum.ErroNoProcessamento;
//                        await _solicitacaoIN100Repository.Update(solicitacao);
//                        if (sol.CreditoExtrato != null && (bool)sol.CreditoExtrato)
//                            await this._usuarioService.EstornarSaldoIN100Extrato(9, (int)sol.UsuarioID, "Estorno IN100/Extrato", sol.NumeroBeneficio);
//                        else
//                            await this._usuarioService.EstornarSaldoIN100(9, (int)sol.UsuarioID, "Estorno IN100", sol.NumeroBeneficio);
//                    }



//                }
//            }
//            return true;

//        }

//        private DadosIN100 TransformaResultado(ResultadoIN100 resultado)
//        {
//            var banco = "";
//            var beneficio = "";
//            using (StreamReader r = new StreamReader("ListaBancos.json"))
//            {
//                string json = r.ReadToEnd();
//                List<ListaFixa> listaBancos = JsonConvert.DeserializeObject<List<ListaFixa>>(json);
//                if (resultado.InstituicaoFinanceiraID != null && resultado.InstituicaoFinanceiraID > 0)
//                {
//                    var bancoLista = listaBancos.Where(x => int.Parse(x.Codigo) == resultado.InstituicaoFinanceiraID).FirstOrDefault();

//                    if (bancoLista != null)
//                    {
//                        banco = $"{bancoLista.Codigo} - {bancoLista.Nome}";
//                    }
//                }
//            }

//            using (StreamReader r = new StreamReader("ListaBeneficios.json"))
//            {
//                string json = r.ReadToEnd();
//                List<ListaFixa> listaBeneficios = JsonConvert.DeserializeObject<List<ListaFixa>>(json);
//                if (resultado.BeneficioID != null && resultado.BeneficioID > 0)
//                {
//                    var beneficioLista = listaBeneficios.Where(x => int.Parse(x.Codigo) == resultado.BeneficioID).FirstOrDefault();

//                    if (beneficioLista != null)
//                    {
//                        beneficio = $"{beneficioLista.Codigo} - {beneficioLista.Nome}";
//                    }
//                }
//            }

//            var idade = 0;
//            if (resultado.DataNascimento != null)
//                idade = this.CalcularIdade((DateTime)resultado.DataNascimento);


//            var resp = new DadosIN100()
//            {
//                BeneficiarioID = resultado.BeneficiarioID == null ? 0 : resultado.BeneficiarioID,
//                Cpf = resultado.Cpf == null ? "" : resultado.Cpf,
//                NumeroBeneficio = resultado.NumeroBeneficio == null ? "" : resultado.NumeroBeneficio,
//                Nome = resultado.Nome == null ? "" : resultado.Nome,
//                DataNascimento = resultado.DataNascimento == null ? "" : (resultado.DataNascimento.Value.Day.ToString("D2") + "/" + resultado.DataNascimento.Value.Month.ToString("D2") + "/" + resultado.DataNascimento.Value.Year + $" ({idade} anos)"),
//                EmprestimoBloqueado = resultado.EmprestimoBloqueado == false ? resultado.PossuiRepresentanteLegal == true ? null : "NÃO" : "SIM",
//                EmprestimoElegivel = resultado.PossuiRepresentanteLegal == true ? null : resultado.EmprestimoElegivel == false ? "NÃO" : "SIM",
//                DIB = resultado.DIB == null ? "" : (resultado.DIB.Value.Day.ToString("D2") + "/" + resultado.DIB.Value.Month.ToString("D2") + "/" + resultado.DIB.Value.Year),
//                BeneficioID = resultado.BeneficioID == null ? 0 : resultado.BeneficioID,
//                MargemConsignavel = resultado.MargemConsignavel == null ? "0" : resultado.PossuiRepresentanteLegal == true ? null : resultado.MargemConsignavel?.ToString("c2"),
//                RMCAtivo = resultado.RMCAtivo == null ? 0 : resultado.RMCAtivo,
//                UFBeneficio = resultado.UFBeneficio == null ? "" : resultado.UFBeneficio,
//                MeioPagamentoID = resultado.MeioPagamentoID == null ? 0 : resultado.MeioPagamentoID,
//                InstituicaoFinanceiraID = resultado.InstituicaoFinanceiraID == null ? 0 : resultado.InstituicaoFinanceiraID,
//                NomeAgencia = resultado.NomeAgencia == null ? "" : resultado.NomeAgencia,
//                NumeroAgencia = resultado.NumeroAgencia == null ? "" : resultado.NumeroAgencia,
//                NumeroContaCorrente = resultado.NumeroContaCorrente == null ? "" : resultado.NumeroContaCorrente,
//                DataAtualizacao = resultado.DataAtualizacao == null ? "" : resultado.DataAtualizacao.Value.ToString("DD/MM/YYYY"),
//                MensagemServidor = resultado.MensagemServidor == null ? "Processado com sucesso." : resultado.MensagemServidor,
//                DDB = resultado.DDB == null ? "" : (resultado.DDB.Value.Day.ToString("D2") + "/" + resultado.DDB.Value.Month.ToString("D2") + "/" + resultado.DDB.Value.Year),
//                RequisicaoID = resultado.RequisicaoID == null ? 0 : resultado.RequisicaoID,
//                Situacao = resultado.Situacao == null ? "" : resultado.Situacao,
//                OrigemBancoID = resultado.OrigemBancoID == null ? 0 : resultado.OrigemBancoID,
//                MargemConsignavelCartao = resultado.MargemConsignavelCartao == null ? "0" : resultado.PossuiRepresentanteLegal == true ? null : resultado.MargemConsignavelCartao?.ToString("c2"),
//                QtdEmprestimosAtivosSuspensos = resultado.QtdEmprestimosAtivosSuspensos == null ? "0" : resultado.QtdEmprestimosAtivosSuspensos?.ToString(),
//                PossuiRepresentanteLegal = resultado.PossuiRepresentanteLegal == true ? "SIM" : "NÃO",
//                PossuiProcurador = resultado.PossuiRepresentanteLegal == true ? null : resultado.PossuiProcurador == true ? "SIM" : "NÃO",
//                Idade = resultado.Idade == null ? 0 : resultado.Idade,
//                MeioPagamento = resultado.MeioPagamentoID == 1 ? "Conta Corrente" : resultado.MeioPagamentoID == 2 ? "Cartão Magnético" : "",
//                Banco = banco,
//                Beneficio = beneficio,
//                MargemNula = (resultado.EmprestimoBloqueado == true || resultado.PossuiRepresentanteLegal == true)
//            };

//            return resp;

//        }

//        private int CalcularIdade(DateTime birthdate)
//        {
//            var today = DateTime.Now;
//            var age = today.Year - birthdate.Year;
//            if (birthdate > today.AddYears(-age))
//                age--;

//            return age;
//        }

//        private ResultadoOfflineDTO TransformaResultadoOffline(List<ResultadoOffline> resultados)
//        {
//            var resultado = resultados[0];

//            var idade = 0;
//            if (resultado.DataNascimento != null)
//                idade = this.CalcularIdade((DateTime)resultado.DataNascimento);

//            var resp = new ResultadoOfflineDTO()
//            {
//                Cpf = resultado.Cpf == null ? " " : resultado.Cpf,
//                Nome = resultado.Nome == null ? " " : resultado.Nome,
//                DataNascimento = resultado.DataNascimento == null ? " " : (resultado.DataNascimento.Value.Day.ToString("D2") + "/" + resultado.DataNascimento.Value.Month.ToString("D2") + "/" + resultado.DataNascimento.Value.Year + $" ({idade} anos)"),
//                Idade = resultado.Idade,
//                RG = resultado.RG == null ? " " : resultado.RG,
//                Sexo = resultado.Sexo == null ? " " : resultado.Sexo == "M" ? "Masculino" : "Feminino",
//                NomeMae = resultado.NomeMae == null ? " " : resultado.NomeMae,
//                Banco = resultado.Banco == null ? " " : resultado.Banco,
//                NumeroAgencia = resultado.NumeroAgencia == null ? " " : resultado.NumeroAgencia,
//                NumeroContaCorrente = resultado.NumeroContaCorrente == null ? " " : resultado.NumeroContaCorrente,
//                MeioPagamento = resultado.MeioPagamento == null ? " " : resultado.MeioPagamento,
//                NumeroBeneficio = resultado.NumeroBeneficio == null ? " " : resultado.NumeroBeneficio,
//                CodigoTipoBeneficio = resultado.CodigoTipoBeneficio.ToString(),
//                DescricaoTipoBeneficio = resultado.CodigoTipoBeneficio.ToString() + " - " + resultado.DescricaoTipoBeneficio,
//                SituacaoBeneficio = resultado.SituacaoBeneficio == null ? " " : resultado.SituacaoBeneficio,
//                EmprestimoBloqueado = resultado.EmprestimoBloqueado == null ? " " : resultado.EmprestimoBloqueado == true ? "SIM" : "NÃO",
//                DescricaoEmprestimoBloqueado = resultado.DescricaoEmprestimoBloqueado == null ? " " : resultado.DescricaoEmprestimoBloqueado,
//                RepresentanteProcurador = resultado.RepresentanteProcurador == null ? " " : resultado.RepresentanteProcurador == true ? "SIM" : "NÃO",
//                DescricaoRepresentanteProcurador = resultado.DescricaoRepresentanteProcurador == null ? " " : resultado.DescricaoRepresentanteProcurador,
//                PossuiRepresentante = resultado.PossuiRepresentante == null ? " " : resultado.PossuiRepresentante == true ? "SIM" : "NÃO",
//                DescricaoPossuiRepresentante = resultado.DescricaoPossuiRepresentante == null ? " " : resultado.DescricaoPossuiRepresentante,
//                CpfRepresentanteLegal = resultado.CpfRepresentanteLegal == null ? " " : resultado.CpfRepresentanteLegal,
//                NomeRepresentanteLegal = resultado.NomeRepresentanteLegal == null ? " " : resultado.NomeRepresentanteLegal,
//                PossuiProcurador = resultado.PossuiProcurador == null ? " " : resultado.PossuiProcurador == true ? "SIM" : "NÃO",
//                DescricaoPossuiProcurador = resultado.DescricaoPossuiProcurador == null ? " " : resultado.DescricaoPossuiProcurador,
//                PensaoAlimenticia = resultado.PensaoAlimenticia == null ? " " : resultado.PensaoAlimenticia == true ? "SIM" : "NÃO",
//                DescricaoPensaoAlimenticia = resultado.DescricaoPensaoAlimenticia,
//                PermiteEmprestimo = resultado.PermiteEmprestimo == null ? " " : resultado.PermiteEmprestimo == true ? "SIM" : "NÃO",
//                DescricaoPermiteEmprestimo = resultado.DescricaoPermiteEmprestimo == null ? " " : resultado.DescricaoPermiteEmprestimo,
//                DIB = resultado.DIB == null ? " " : (resultado.DIB.Value.Day.ToString("D2") + "/" + resultado.DIB.Value.Month.ToString("D2") + "/" + resultado.DIB.Value.Year),
//                DDB = resultado.DDB == null ? " " : (resultado.DDB.Value.Day.ToString("D2") + "/" + resultado.DDB.Value.Month.ToString("D2") + "/" + resultado.DDB.Value.Year),
//                Competencia = resultado.Competencia == null ? " " : resultado.Competencia,
//                UFBeneficio = resultado.UFBeneficio == null ? " " : resultado.UFBeneficio,
//                SalarioBruto = resultado.SalarioBruto == null ? "0" : resultado.SalarioBruto?.ToString("c2"),
//                SalarioBase = resultado.SalarioBase == null ? "0" : resultado.SalarioBase?.ToString("c2"),
//                MargemEmprestimo = resultado.MargemEmprestimo == null ? "0" : resultado.MargemEmprestimo?.ToString("c2"),
//                ValorMargemEmprestimo = resultado.MargemEmprestimo == null ? 0 : resultado.MargemEmprestimo,
//                ValorMargemAumentoSalarial = resultado.MargemAumentoSalarial == null ? 0 : resultado.MargemAumentoSalarial,
//                MargemAumentoSalarial = resultado.MargemAumentoSalarial == null ? "0" : resultado.MargemAumentoSalarial?.ToString("c2"),
//                Endereco = resultado.Endereco == null ? " " : resultado.Endereco,
//                Cidade = resultado.Cidade == null ? " " : resultado.Cidade,
//                UF = resultado.UF == null ? " " : resultado.UF,
//                CEP = resultado.CEP == null ? " " : resultado.CEP,
//                Telefone1 = resultado.Telefone1 == null ? null : resultado.Telefone1.Length > 10 ? long.Parse(resultado.Telefone1).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone1).ToString(@"(00) 0000-0000"),
//                Telefone2 = resultado.Telefone2 == null ? null : resultado.Telefone2.Length > 10 ? long.Parse(resultado.Telefone2).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone2).ToString(@"(00) 0000-0000"),
//                Telefone3 = resultado.Telefone3 == null ? null : resultado.Telefone3.Length > 10 ? long.Parse(resultado.Telefone3).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone3).ToString(@"(00) 0000-0000"),
//                Telefone4 = resultado.Telefone4 == null ? null : resultado.Telefone4.Length > 10 ? long.Parse(resultado.Telefone4).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone4).ToString(@"(00) 0000-0000"),
//                Telefone5 = resultado.Telefone5 == null ? null : resultado.Telefone5.Length > 10 ? long.Parse(resultado.Telefone5).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone5).ToString(@"(00) 0000-0000"),
//                Telefone6 = resultado.Telefone6 == null ? null : resultado.Telefone6.Length > 10 ? long.Parse(resultado.Telefone6).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone6).ToString(@"(00) 0000-0000"),
//                Telefone7 = resultado.Telefone7 == null ? null : resultado.Telefone7.Length > 10 ? long.Parse(resultado.Telefone7).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone7).ToString(@"(00) 0000-0000"),
//                Telefone8 = resultado.Telefone8 == null ? null : resultado.Telefone8.Length > 10 ? long.Parse(resultado.Telefone8).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone8).ToString(@"(00) 0000-0000"),
//                Telefone9 = resultado.Telefone9 == null ? null : resultado.Telefone9.Length > 10 ? long.Parse(resultado.Telefone9).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone9).ToString(@"(00) 0000-0000"),
//                Telefone10 = resultado.Telefone10 == null ? null : resultado.Telefone10.Length > 10 ? long.Parse(resultado.Telefone10).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone10).ToString(@"(00) 0000-0000"),
//                Telefone11 = resultado.Telefone11 == null ? null : resultado.Telefone11.Length > 10 ? long.Parse(resultado.Telefone11).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone11).ToString(@"(00) 0000-0000"),
//                Telefone12 = resultado.Telefone12 == null ? null : resultado.Telefone12.Length > 10 ? long.Parse(resultado.Telefone12).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone12).ToString(@"(00) 0000-0000"),
//                Telefone13 = resultado.Telefone13 == null ? null : resultado.Telefone13.Length > 10 ? long.Parse(resultado.Telefone13).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone13).ToString(@"(00) 0000-0000"),
//                Telefone14 = resultado.Telefone14 == null ? null : resultado.Telefone14.Length > 10 ? long.Parse(resultado.Telefone14).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone14).ToString(@"(00) 0000-0000"),
//                Telefone15 = resultado.Telefone15 == null ? null : resultado.Telefone15.Length > 10 ? long.Parse(resultado.Telefone15).ToString(@"(00) 00000-0000") : long.Parse(resultado.Telefone15).ToString(@"(00) 0000-0000"),
//                WhatsApp1 = resultado.WhatsApp1 == null ? null : long.Parse(resultado.WhatsApp1).ToString(@"(00) 00000-0000"),
//                WhatsApp2 = resultado.WhatsApp2 == null ? null : long.Parse(resultado.WhatsApp2).ToString(@"(00) 00000-0000"),
//                WhatsApp3 = resultado.WhatsApp3 == null ? null : long.Parse(resultado.WhatsApp3).ToString(@"(00) 00000-0000"),
//                WhatsApp4 = resultado.WhatsApp4 == null ? null : long.Parse(resultado.WhatsApp4).ToString(@"(00) 00000-0000"),
//                WhatsApp5 = resultado.WhatsApp5 == null ? null : long.Parse(resultado.WhatsApp5).ToString(@"(00) 00000-0000"),
//                WhatsApp6 = resultado.WhatsApp6 == null ? null : long.Parse(resultado.WhatsApp6).ToString(@"(00) 00000-0000"),
//                WhatsApp7 = resultado.WhatsApp7 == null ? null : long.Parse(resultado.WhatsApp7).ToString(@"(00) 00000-0000"),
//                WhatsApp8 = resultado.WhatsApp8 == null ? null : long.Parse(resultado.WhatsApp8).ToString(@"(00) 00000-0000"),
//                WhatsApp9 = resultado.WhatsApp9 == null ? null : long.Parse(resultado.WhatsApp9).ToString(@"(00) 00000-0000"),
//                WhatsApp10 = resultado.WhatsApp10 == null ? null : long.Parse(resultado.WhatsApp10).ToString(@"(00) 00000-0000"),
//                WhatsApp11 = resultado.WhatsApp11 == null ? null : long.Parse(resultado.WhatsApp11).ToString(@"(00) 00000-0000"),
//                WhatsApp12 = resultado.WhatsApp12 == null ? null : long.Parse(resultado.WhatsApp12).ToString(@"(00) 00000-0000"),
//                WhatsApp13 = resultado.WhatsApp13 == null ? null : long.Parse(resultado.WhatsApp13).ToString(@"(00) 00000-0000"),
//                WhatsApp14 = resultado.WhatsApp14 == null ? null : long.Parse(resultado.WhatsApp14).ToString(@"(00) 00000-0000"),
//                WhatsApp15 = resultado.WhatsApp15 == null ? null : long.Parse(resultado.WhatsApp15).ToString(@"(00) 00000-0000"),
//                Email1 = resultado.Email1,
//                Email2 = resultado.Email2,
//                Email3 = resultado.Email3,
//                DataAtualizacao = resultado.DataAtualizacao == null ? "" : (resultado.DataAtualizacao.Value.Day.ToString("D2") + "/" + resultado.DataAtualizacao.Value.Month.ToString("D2") + "/" + resultado.DataAtualizacao.Value.Year),
//                LimiteCartaoRMC = 0.ToString("c2"),
//                MargemCartaoRMC = 0.ToString("c2"),
//                ValorLiberadoCartaoRMC = 0.ToString("c2"),
//                LimiteCartaoRCC = 0.ToString("c2"),
//                MargemCartaoRCC = 0.ToString("c2"),
//                ValorLiberadoCartaoRCC = 0.ToString("c2"),
//                MargemCartaoBeneficio = 0.ToString("c2"),
//                BancosEmprestimos = new List<BancoEmprestimoDTO>(),
//            };

//            resp.ListaTelefones = ListaTelefones(resp);
//            resp.ListaWhatsapps = ListaWhatsapps(resp);
//            resp.ListaEmails = ListaEmails(resp);


//            //if(resultado.MargemCartaoBeneficio != null && resultado.MargemCartaoBeneficio > 0)
//            //{
//            //    resp.MargemCartaoRMC = resultado.MargemCartaoBeneficio?.ToString("c2"); ;
//            //    resp.LimiteCartaoRMC = ((double)resultado.MargemCartaoBeneficio * 27.5).ToString("c2");
//            //    resp.ValorLiberadoCartaoRMC = ((double)resultado.MargemCartaoBeneficio * 27.5 * 0.7).ToString("c2");
//            //}
//            //else if( (resultado.MargemCartaoBeneficio == null || resultado.MargemCartaoBeneficio == 0) && (resultado.MargemCartao != null && resultado.MargemCartao > 0))
//            //{
//            //    resp.MargemCartaoRMC = resultado.MargemCartao?.ToString("c2"); ;
//            //    resp.LimiteCartaoRMC = ((double)resultado.MargemCartao * 27.5).ToString("c2");
//            //    resp.ValorLiberadoCartaoRMC = ((double)resultado.MargemCartao * 27.5 * 0.7).ToString("c2");

//            //}
//            //if(resultado.MargemCartao != null && resultado.MargemCartao > 0)
//            //{
//            //    resp.MargemCartaoRCC = resultado.MargemCartao?.ToString("c2"); ;
//            //    resp.LimiteCartaoRCC = ((double)resultado.MargemCartao * 27.5).ToString("c2");
//            //    resp.ValorLiberadoCartaoRCC = ((double)resultado.MargemCartao * 27.5 * 0.7).ToString("c2");
//            //}
//            //else if ((resultado.MargemCartao == null || resultado.MargemCartao == 0) && (resultado.MargemCartaoBeneficio != null && resultado.MargemCartaoBeneficio > 0))
//            //{
//            //    resp.MargemCartaoRCC = resultado.MargemCartaoBeneficio?.ToString("c2"); ;
//            //    resp.LimiteCartaoRCC = ((double)resultado.MargemCartaoBeneficio * 27.5).ToString("c2");
//            //    resp.ValorLiberadoCartaoRCC = ((double)resultado.MargemCartaoBeneficio * 27.5 * 0.7).ToString("c2");

//            //}

//            foreach (var res in resultados)
//            {
//                if (res.DescricaoTipoEmprestimo != null && res.DescricaoTipoEmprestimo == "CONSIGNADO" && res.BancoEmprestimo != null)
//                {
//                    var bancoEmprestimo = new BancoEmprestimoDTO
//                    {
//                        BancoEmprestimo = res.BancoEmprestimo,
//                        EmprestimosDTO = new List<EmprestimoDTO>(),
//                    };
//                    if (!resp.BancosEmprestimos.Any(x => x.BancoEmprestimo == bancoEmprestimo.BancoEmprestimo))
//                    {
//                        resp.BancosEmprestimos.Add(bancoEmprestimo);
//                    }
//                }

//            }


//            foreach (var res in resultados)
//            {
//                if (res.DescricaoTipoEmprestimo != null && res.DescricaoTipoEmprestimo == "CONSIGNADO" && res.BancoEmprestimo != null)
//                {
//                    var emprestimoDTO = new EmprestimoDTO()
//                    {
//                        NumeroContratoEmprestimo = res.NumeroContrato,
//                        DescricaoTipoEmprestimo = res.DescricaoTipoEmprestimo,
//                        SituacaoEmprestimo = res.SituacaoEmprestimo,
//                        ValorParcelaEmprestimo = res.ValorParcela == null ? "0" : res.ValorParcela?.ToString("c2"),
//                        ParcelasPagasEmprestimo = res.ParcelasPagas.ToString(),
//                        PrazoEmprestimo = res.Prazo.ToString(),
//                        CompetenciaInicioDescontoEmprestimo = res.CompetenciaInicioDesconto,
//                        CompetenciaFinalDescontoEmprestimo = res.CompetenciaFinalDesconto,
//                        ValorEmprestimo = res.ValorEmprestimo == null ? "0" : res.ValorEmprestimo?.ToString("c2"),
//                        SaldoEmprestimo = res.Saldo == null ? "0" : res.Saldo?.ToString("c2"),
//                        TaxaEmprestimo = res.Taxa.ToString() + "%",
//                        DataAverbacaoEmprestimo = res.DataAverbacao == null ? "" : (res.DataAverbacao.Value.Day.ToString("D2") + "/" + res.DataAverbacao.Value.Month.ToString("D2") + "/" + res.DataAverbacao.Value.Year),
//                    };

//                    resp.BancosEmprestimos.Where(x => x.BancoEmprestimo == res.BancoEmprestimo).First().EmprestimosDTO.Add(emprestimoDTO);
//                }
//                else if (res.DescricaoTipoEmprestimo != null && res.DescricaoTipoEmprestimo == "CARTÃO")
//                {
//                    if (resp.NumeroContratoCartao1 == null)
//                    {
//                        resp.BancoCartao1 = res.BancoEmprestimo;
//                        resp.NumeroContratoCartao1 = res.NumeroContrato;
//                        resp.SituacaoCartao1 = res.SituacaoCartao;
//                        resp.LimiteCartao1 = res.LimiteCartao == null ? "0" : res.LimiteCartao?.ToString("c2");
//                        resp.ValorReservadoCartao1 = res.ValorReservadoCartao == null ? "0" : res.ValorReservadoCartao?.ToString("c2");
//                        resp.TipoConsignacaoCartao1 = res.TipoConsignacao;
//                        resp.DataAverbacaoCartao1 = res.DataAverbacao == null ? "" : (res.DataAverbacao.Value.Day.ToString("D2") + "/" + res.DataAverbacao.Value.Month.ToString("D2") + "/" + res.DataAverbacao.Value.Year);
//                        resp.MargemCartao = res.MargemCartao == null ? "0" : res.MargemCartao?.ToString("c2");
//                        resp.MargemCartaoBeneficio = res.MargemCartaoBeneficio == null ? "0" : res.MargemCartaoBeneficio?.ToString("c2");

//                        if (resp.TipoConsignacaoCartao1 == "RMC")
//                        {
//                            resp.MargemCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : res.MargemCartaoBeneficio?.ToString("c2");
//                            resp.LimiteCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : ((double)res.MargemCartaoBeneficio * 27.5).ToString("c2");
//                            resp.ValorLiberadoCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : ((double)res.MargemCartaoBeneficio * 27.5 * 0.7).ToString("c2");
//                        }

//                        else if (resp.TipoConsignacaoCartao1 == "RCC")
//                        {
//                            resp.MargemCartaoRCC = res.MargemCartao == null ? "0" : res.MargemCartao?.ToString("c2");
//                            resp.LimiteCartaoRCC = res.MargemCartao == null ? "0" : ((double)res.MargemCartao * 27.5).ToString("c2");
//                            resp.ValorLiberadoCartaoRCC = res.MargemCartao == null ? "0" : ((double)res.MargemCartao * 27.5 * 0.7).ToString("c2");
//                        }
//                    }
//                    else if (resp.NumeroContratoCartao2 == null)
//                    {
//                        resp.BancoCartao2 = res.BancoEmprestimo;
//                        resp.NumeroContratoCartao2 = res.NumeroContrato;
//                        resp.SituacaoCartao2 = res.SituacaoCartao;
//                        resp.LimiteCartao2 = res.LimiteCartao == null ? "0" : res.LimiteCartao?.ToString("c2");
//                        resp.ValorReservadoCartao2 = res.ValorReservadoCartao == null ? "0" : res.ValorReservadoCartao?.ToString("c2");
//                        resp.TipoConsignacaoCartao2 = res.TipoConsignacao;
//                        resp.DataAverbacaoCartao2 = res.DataAverbacao == null ? "" : (res.DataAverbacao.Value.Day.ToString("D2") + "/" + res.DataAverbacao.Value.Month.ToString("D2") + "/" + resultado.DataAverbacao.Value.Year);

//                        if (resp.TipoConsignacaoCartao2 == "RMC")
//                        {
//                            resp.MargemCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : res.MargemCartaoBeneficio?.ToString("c2");
//                            resp.LimiteCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : ((double)res.MargemCartaoBeneficio * 27.5).ToString("c2");
//                            resp.ValorLiberadoCartaoRMC = res.MargemCartaoBeneficio == null ? "0" : ((double)res.MargemCartaoBeneficio * 27.5 * 0.7).ToString("c2");
//                        }

//                        else if (resp.TipoConsignacaoCartao2 == "RCC")
//                        {
//                            resp.MargemCartaoRCC = res.MargemCartao == null ? "0" : res.MargemCartao?.ToString("c2");
//                            resp.LimiteCartaoRCC = res.MargemCartao == null ? "0" : ((double)res.MargemCartao * 27.5).ToString("c2");
//                            resp.ValorLiberadoCartaoRCC = res.MargemCartao == null ? "0" : ((double)res.MargemCartao * 27.5 * 0.7).ToString("c2");
//                        }
//                    }
//                }
//            }

//            resp.ValorMargemAumentoSalarial = CalculaAumentoSalarial(resultado);
//            resp.MargemAumentoSalarial = resp.ValorMargemAumentoSalarial?.ToString("c2");

//            resp.ValorMargemEmprestimo = (float)(CalculaMargemSalario(resp, resultado));
//            resp.MargemEmprestimo = resp.ValorMargemEmprestimo?.ToString("c2");

//            var margemRCC = MargemRCC(resp, resultado);
//            resp.MargemCartaoRCC = margemRCC.ToString("c2");
//            resp.LimiteCartaoRCC = (margemRCC * 27.5).ToString("c2");
//            resp.ValorLiberadoCartaoRCC = (margemRCC * 27.5 * 0.7).ToString("c2");
//            resp.PossivelMargemRCC = (resp.TipoConsignacaoCartao1 == "RCC" || resp.TipoConsignacaoCartao2 == "RCC") && (margemRCC > 0.01);

//            var margemRMC = MargemRMC(resp, resultado, false);
//            resp.MargemCartaoRMC = margemRMC.ToString("c2");
//            resp.LimiteCartaoRMC = (margemRMC * 27.5).ToString("c2");
//            resp.ValorLiberadoCartaoRMC = (margemRMC * 27.5 * 0.7).ToString("c2");
//            resp.PossivelMargemRMC = (resp.TipoConsignacaoCartao1 == "RMC" || resp.TipoConsignacaoCartao2 == "RMC") && (margemRMC > 0.01);

//            if (
//                (resp.TipoConsignacaoCartao1 == "RMC" && DateTime.Parse(resp.DataAverbacaoCartao1) < DateTime.Parse("16/09/2014")) ||
//                (resp.TipoConsignacaoCartao2 == "RMC" && DateTime.Parse(resp.DataAverbacaoCartao2) < DateTime.Parse("16/09/2014"))
//                )
//            {
//                margemRMC = MargemRMC(resp, resultado, true);
//                resp.LimiteCartaoRCC = 0.ToString("c2");
//                resp.MargemCartaoRCC = 0.ToString("c2");
//                resp.ValorLiberadoCartaoRCC = 0.ToString("c2");
//                resp.PossivelMargemRCC = false;
//                resp.RegraDataAverbacaoRCC = true;
//                resp.MargemCartaoRMC = (margemRMC * (-1)).ToString("c2");
//                resp.LimiteCartaoRMC = (margemRMC * 27.5 * (-1)).ToString("c2");
//                resp.ValorLiberadoCartaoRMC = (margemRMC * 27.5 * 0.7 * (-1)).ToString("c2");
//                resp.PossivelMargemRMC = false;
//            }

//            return resp;

//        }
//        private float CalculaAumentoSalarial(ResultadoOffline resultado)
//        {
//            double salarioMinimo = 1212;
//            double taxaMenor = 0.074257 ;
//            double taxaMaior = 0.0592;
//            double taxaPermitida = 0.35;

//            double aumentoMinimo = salarioMinimo * taxaMenor;
//            double salarioMinimoNovo = salarioMinimo + (aumentoMinimo);

//            if (resultado.SalarioBase != null && resultado.SalarioBase > 0)
//            {
//                double aumentoSalario = 0;
//                double salarioTotalNovo = 0;

//                if (resultado.SalarioBase > salarioMinimo)
//                {
//                    aumentoSalario = (double)resultado.SalarioBase * taxaMaior;
//                    salarioTotalNovo = (double)resultado.SalarioBase + aumentoSalario;

//                    if (salarioTotalNovo >= salarioMinimoNovo)
//                        return (float)((aumentoSalario * taxaPermitida) - 0.05);
//                    else
//                        return (float)((aumentoMinimo * taxaPermitida) - 0.05);

//                }
//                else
//                {
//                    aumentoSalario = (double)resultado.SalarioBase * taxaMenor;
//                    salarioTotalNovo = (double)resultado.SalarioBase + aumentoSalario;
//                    return (float)((aumentoSalario * taxaPermitida) - 0.05);
//                }
//            }
//            else if (resultado.SalarioBruto != null && resultado.SalarioBruto > 0)
//            {
//                double aumentoSalario = 0;
//                double salarioTotalNovo = 0;

//                if (resultado.SalarioBruto > salarioMinimo)
//                {
//                    aumentoSalario = (double)resultado.SalarioBruto * taxaMaior;
//                    salarioTotalNovo = (double)resultado.SalarioBruto + aumentoSalario;

//                    if (salarioTotalNovo >= salarioMinimoNovo)
//                        return (float)((aumentoSalario * taxaPermitida) - 0.05);
//                    else
//                        return (float)((aumentoMinimo * taxaPermitida) - 0.05);

//                }
//                else
//                {
//                    aumentoSalario = (double)resultado.SalarioBruto * taxaMenor;
//                    salarioTotalNovo = (double)resultado.SalarioBruto + aumentoSalario;
//                    return (float)((aumentoSalario * taxaPermitida) - 0.05);
//                }
//            }
//            else
//            {
//                return 0;
//            }

//        }

//        private double CalculaMargemSalario(ResultadoOfflineDTO resp, ResultadoOffline result)
//        {
//            double margemEmprestimo = 0;

//            if (result.SalarioBase != null && result.SalarioBase > 0)
//            {
//                margemEmprestimo = (double)(result.SalarioBase * 0.35);

//                foreach(var bancoEmprestimo in resp.BancosEmprestimos)
//                {
//                    foreach(var emprestimo in bancoEmprestimo.EmprestimosDTO)
//                    {
//                        margemEmprestimo = margemEmprestimo - double.Parse(emprestimo.ValorParcelaEmprestimo.Replace("R$", ""));
//                    }
//                }
//            }
//            else if (result.SalarioBruto != null && result.SalarioBruto > 0)
//            {
//                margemEmprestimo = (double)(result.SalarioBruto * 0.35);

//                foreach (var bancoEmprestimo in resp.BancosEmprestimos)
//                {
//                    foreach (var emprestimo in bancoEmprestimo.EmprestimosDTO)
//                    {
//                        margemEmprestimo = margemEmprestimo - double.Parse(emprestimo.ValorParcelaEmprestimo.Replace("R$", ""));
//                    }
//                }
//            }


//            return margemEmprestimo;

//        }

//        private double MargemRCC(ResultadoOfflineDTO resp, ResultadoOffline result)
//        {
//            double margemCartao = 0;

//            var rmcAtivo = resp.NumeroContratoCartao1 != null || resp.NumeroContratoCartao2 != null;

//            if (rmcAtivo && result.SalarioBase != null && result.SalarioBase > 0)
//            {
//                margemCartao = (double)(result.SalarioBase * 0.05);

//                if(resp.NumeroContratoCartao1 != null && resp.TipoConsignacaoCartao1 == "RCC")
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao1.Replace("R$", ""));
//                }
//                if (resp.NumeroContratoCartao2 != null && resp.TipoConsignacaoCartao2 == "RCC")
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao2.Replace("R$", ""));
//                }
//            }
//            else if (rmcAtivo && result.SalarioBruto != null && result.SalarioBruto > 0)
//            {
//                margemCartao = (double)(result.SalarioBruto * 0.05);

//                if (resp.NumeroContratoCartao1 != null && resp.TipoConsignacaoCartao1 == "RCC")
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao1.Replace("R$", ""));
//                }
//                if (resp.NumeroContratoCartao2 != null && resp.TipoConsignacaoCartao2 == "RCC")
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao2.Replace("R$", ""));
//                }
//            }
//            else if (!rmcAtivo && result.SalarioBase != null && result.SalarioBase > 0)
//            {
//                margemCartao = (double)(result.SalarioBase * 0.05);
//            }
//            else if (!rmcAtivo && result.SalarioBruto != null && result.SalarioBruto > 0)
//            {
//                margemCartao = (double)(result.SalarioBruto * 0.05);
//            }


//            if (margemCartao <= 0.1 && margemCartao >= -0.1)
//                margemCartao = 0;
                

//            return margemCartao;

//        }

//        private double MargemRMC(ResultadoOfflineDTO resp, ResultadoOffline result, bool regraDataAverbacao)
//        {
//            double margemCartao = 0;

//            var rmcAtivo = resp.NumeroContratoCartao1 != null || resp.NumeroContratoCartao2 != null;

//            if (rmcAtivo && result.SalarioBase != null && result.SalarioBase > 0)
//            {
//                margemCartao = (double)(result.SalarioBase * 0.05);

//                if (resp.NumeroContratoCartao1 != null && resp.TipoConsignacaoCartao1 == "RMC" && !regraDataAverbacao)
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao1.Replace("R$", ""));
//                }
//                if (resp.NumeroContratoCartao2 != null && resp.TipoConsignacaoCartao2 == "RMC" && !regraDataAverbacao)
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao2.Replace("R$", ""));
//                }
//            }
//            else if (rmcAtivo && result.SalarioBruto != null && result.SalarioBruto > 0)
//            {
//                margemCartao = (double)(result.SalarioBruto * 0.05);

//                if (resp.NumeroContratoCartao1 != null && resp.TipoConsignacaoCartao1 == "RMC" && !regraDataAverbacao)
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao1.Replace("R$", ""));
//                }
//                if (resp.NumeroContratoCartao2 != null && resp.TipoConsignacaoCartao2 == "RMC" && !regraDataAverbacao)
//                {
//                    margemCartao = margemCartao - double.Parse(resp.ValorReservadoCartao2.Replace("R$", ""));
//                }
//            }
//            else if(!rmcAtivo && result.SalarioBase != null && result.SalarioBase > 0)
//            {
//                margemCartao = (double)(result.SalarioBase * 0.05);
//            }
//            else if (!rmcAtivo && result.SalarioBruto != null && result.SalarioBruto > 0)
//            {
//                margemCartao = (double)(result.SalarioBruto * 0.05);
//            }


//            if (margemCartao <= 0.1 && margemCartao >= -0.1)
//                margemCartao = 0;


//            return margemCartao;

//        }

//        private List<string> ListaTelefones(ResultadoOfflineDTO resultado)
//        {
//            var listTelefones = new List<string>();

//            if(resultado.Telefone1 != null && !listTelefones.Contains(resultado.Telefone1))
//                listTelefones.Add(resultado.Telefone1);
//            if (resultado.Telefone2 != null && !listTelefones.Contains(resultado.Telefone2))
//                listTelefones.Add(resultado.Telefone2);
//            if (resultado.Telefone3 != null && !listTelefones.Contains(resultado.Telefone3))
//                listTelefones.Add(resultado.Telefone3);
//            if (resultado.Telefone4 != null && !listTelefones.Contains(resultado.Telefone4))
//                listTelefones.Add(resultado.Telefone4);
//            if (resultado.Telefone5 != null && !listTelefones.Contains(resultado.Telefone5))
//                listTelefones.Add(resultado.Telefone5);
//            if (resultado.Telefone6 != null && !listTelefones.Contains(resultado.Telefone6))
//                listTelefones.Add(resultado.Telefone6);
//            if (resultado.Telefone7 != null && !listTelefones.Contains(resultado.Telefone7))
//                listTelefones.Add(resultado.Telefone7);
//            if (resultado.Telefone8 != null && !listTelefones.Contains(resultado.Telefone8))
//                listTelefones.Add(resultado.Telefone8);
//            if (resultado.Telefone9 != null && !listTelefones.Contains(resultado.Telefone9))
//                listTelefones.Add(resultado.Telefone9);
//            if (resultado.Telefone10 != null && !listTelefones.Contains(resultado.Telefone10))
//                listTelefones.Add(resultado.Telefone1);
//            if (resultado.Telefone11 != null && !listTelefones.Contains(resultado.Telefone11))
//                listTelefones.Add(resultado.Telefone11);
//            if (resultado.Telefone12 != null && !listTelefones.Contains(resultado.Telefone12))
//                listTelefones.Add(resultado.Telefone12);
//            if (resultado.Telefone13 != null && !listTelefones.Contains(resultado.Telefone13))
//                listTelefones.Add(resultado.Telefone13);
//            if (resultado.Telefone14 != null && !listTelefones.Contains(resultado.Telefone14))
//                listTelefones.Add(resultado.Telefone14);
//            if (resultado.Telefone15 != null && !listTelefones.Contains(resultado.Telefone15))
//                listTelefones.Add(resultado.Telefone15);

//            return listTelefones;

//        }

//        private List<string> ListaWhatsapps(ResultadoOfflineDTO resultado)
//        {
//            var listWhatsapps = new List<string>();

//            if (resultado.WhatsApp1 != null && !listWhatsapps.Contains(resultado.WhatsApp1))
//                listWhatsapps.Add(resultado.WhatsApp1);
//            if (resultado.WhatsApp2 != null && !listWhatsapps.Contains(resultado.WhatsApp2))
//                listWhatsapps.Add(resultado.WhatsApp2);
//            if (resultado.WhatsApp3 != null && !listWhatsapps.Contains(resultado.WhatsApp3))
//                listWhatsapps.Add(resultado.WhatsApp3);
//            if (resultado.WhatsApp4 != null && !listWhatsapps.Contains(resultado.WhatsApp4))
//                listWhatsapps.Add(resultado.WhatsApp4);
//            if (resultado.WhatsApp5 != null && !listWhatsapps.Contains(resultado.WhatsApp5))
//                listWhatsapps.Add(resultado.WhatsApp5);
//            if (resultado.WhatsApp6 != null && !listWhatsapps.Contains(resultado.WhatsApp6))
//                listWhatsapps.Add(resultado.WhatsApp6);
//            if (resultado.WhatsApp7 != null && !listWhatsapps.Contains(resultado.WhatsApp7))
//                listWhatsapps.Add(resultado.WhatsApp7);
//            if (resultado.WhatsApp8 != null && !listWhatsapps.Contains(resultado.WhatsApp8))
//                listWhatsapps.Add(resultado.WhatsApp7);
//            if (resultado.WhatsApp8 != null && !listWhatsapps.Contains(resultado.WhatsApp8))
//                listWhatsapps.Add(resultado.WhatsApp8);
//            if (resultado.WhatsApp9 != null && !listWhatsapps.Contains(resultado.WhatsApp9))
//                listWhatsapps.Add(resultado.WhatsApp9);
//            if (resultado.WhatsApp10 != null && !listWhatsapps.Contains(resultado.WhatsApp10))
//                listWhatsapps.Add(resultado.WhatsApp10);
//            if (resultado.WhatsApp11 != null && !listWhatsapps.Contains(resultado.WhatsApp11))
//                listWhatsapps.Add(resultado.WhatsApp11);
//            if (resultado.WhatsApp12 != null && !listWhatsapps.Contains(resultado.WhatsApp12))
//                listWhatsapps.Add(resultado.WhatsApp12);
//            if (resultado.WhatsApp13 != null && !listWhatsapps.Contains(resultado.WhatsApp13))
//                listWhatsapps.Add(resultado.WhatsApp13);
//            if (resultado.WhatsApp14 != null && !listWhatsapps.Contains(resultado.WhatsApp14))
//                listWhatsapps.Add(resultado.WhatsApp1);
//            if (resultado.WhatsApp15 != null && !listWhatsapps.Contains(resultado.WhatsApp15))
//                listWhatsapps.Add(resultado.WhatsApp15);

//            return listWhatsapps;

//        }

//        private List<string> ListaEmails(ResultadoOfflineDTO resultado)
//        {
//            var listEmails = new List<string>();

//            if (resultado.Email1 != null && !listEmails.Contains(resultado.Email1))
//                listEmails.Add(resultado.Email1);
//            if (resultado.Email2 != null && !listEmails.Contains(resultado.Email2))
//                listEmails.Add(resultado.Email2);
//            if (resultado.Email3 != null && !listEmails.Contains(resultado.Email3))
//                listEmails.Add(resultado.Email3);

//            return listEmails;

//        }
//    }

//}