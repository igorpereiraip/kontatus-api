
//using AutoMapper;
//using Kontatus.API.Configurations;
//using Kontatus.Domain.DTO;
//using Kontatus.Domain.Entity;
//using Kontatus.Helper.Utilitarios;
//using Kontatus.Service;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Claims;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Kontatus.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
//    [ApiController]
//    public class PesquisaController : ControllerBase
//    {
//        private readonly IMapper mapper;
//        private readonly IPesquisaService _service;
//        private readonly IUsuarioService _usuarioService;
//        private readonly IHttpContextAccessor httpContextAccessor;
//        private readonly IArquivoService _arquivoService;

//        public PesquisaController(IPesquisaService service, IMapper mapper, IUsuarioService usuarioService, IHttpContextAccessor _httpContextAccessor, IArquivoService arquivoService)
//        {
//            this.mapper = mapper;
//            _service = service;
//            _usuarioService = usuarioService;
//            httpContextAccessor = _httpContextAccessor;
//            _arquivoService = arquivoService;
//        }

//        [HttpGet("pesquisaBeneficiosByCpf/{cpf}")]
//        public async Task<Result<PesquisaBeneficio>> PesquisaBeneficiosByCpf(string cpf)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if(usuario.ValidadePlano != null && usuario.ValidadePlano < DateTime.Now)
//                {
//                    throw new Exception("Plano expirado. Procure seu comercial e renove para continuar utilizando o sistema.");
//                }

//                var pesquisaBeneficio = await _service.GetDadosBeneficiosByCpfAsync(cpf);
//                return Result<PesquisaBeneficio>.Ok(pesquisaBeneficio);
//            }
//            catch (Exception ex)
//            {
//                return Result<PesquisaBeneficio>.Err(ex.Message);
//            }
//        }

//        [HttpGet("pesquisaExtratoOffline/{beneficio}")]
//        public async Task<Result<ResultadoOfflineDTO>> PesquisaExtratoOffline(string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.SaldoOffline <= 0 && (usuario.OfflineIlimitado == null || (bool)usuario.OfflineIlimitado == false))
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var pesquisaBeneficio = await _service.PesquisaExtratoOffline(beneficio, usuarioID);

//                var consultaDiariaBeneficio = await _service.VerificaPesquisaBeneficioDiaria(beneficio, usuarioID);
//                if (usuario.SaldoOffline > 0)
//                    await _usuarioService.ConsumirSaldoOffline(loginID, usuarioID, "Offline", beneficio, consultaDiariaBeneficio);
//                else
//                    await _usuarioService.ConsumirSaldoOfflineIlimitado(loginID, usuarioID, "Offline Ilimitado", beneficio);

                

//                return Result<ResultadoOfflineDTO>.Ok(pesquisaBeneficio);
//            }
//            catch (Exception ex)
//            {
//                return Result<ResultadoOfflineDTO>.Err(ex.Message);
//            }
//        }


//        [HttpGet("pesquisaBeneficiosByBeneficio/{beneficio}")]
//        public async Task<Result<PesquisaBeneficio>> PesquisaBeneficiosByBeneficio(string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.ValidadePlano != null && usuario.ValidadePlano < DateTime.Now)
//                {
//                    throw new Exception("Plano expirado. Procure seu comercial e renove para continuar utilizando o sistema.");
//                }

//                var pesquisaBeneficio = await _service.GetDadosBeneficiosByBeneficioAsync(beneficio);
//                return Result<PesquisaBeneficio>.Ok(pesquisaBeneficio);
//            }
//            catch (Exception ex)
//            {
//                return Result<PesquisaBeneficio>.Err(ex.Message);
//            }
//        }

//        [HttpGet("ArquivoExtrato2/{beneficio}")]
//        public async Task<FileContentResult> ArquivoExtrato2(string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.SaldoExtrato <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var (conteudo, nome) = await _service.ArquivoExtrato2(beneficio);
//                var file = File(conteudo, MimeMapping.GetMapping(nome), nome);

//                var stream = new MemoryStream(conteudo);
//                IFormFile formFile = new FormFile(stream, 0, conteudo.Length, nome, nome);

//                var arquivos = new List<IFormFile>();
//                arquivos.Add(formFile);

//                var arquivoDto = new ArquivoDTO()
//                {
//                    Beneficio = beneficio,
//                    UsuarioID = usuarioID,
//                    Mes = DateTime.Now.Month.ToString("d2"),
//                    Ano = DateTime.Now.Day.ToString("d2"),
//                    Arquivos = arquivos
//                };

//                await _arquivoService.SalvarArquivo(arquivoDto);

//                var usuarioLog = _usuarioService.ConsumirSaldoExtrato(loginID, usuarioID, "Extrato Novo", beneficio);
//                return file;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }


//        [HttpGet("ArquivoExtrato1/{beneficio}")]
//        public async Task<FileContentResult> ArquivoExtrato1(string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.SaldoExtrato <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var (conteudo, nome) = await _service.ArquivoExtrato1(beneficio);
//                var file = File(conteudo, MimeMapping.GetMapping(nome), nome);

//                var stream = new MemoryStream(conteudo);
//                IFormFile formFile = new FormFile(stream, 0, conteudo.Length, nome, nome);

//                var arquivos = new List<IFormFile>();
//                arquivos.Add(formFile);

//                var arquivoDto = new ArquivoDTO()
//                {
//                    Beneficio = beneficio,
//                    UsuarioID = usuarioID,
//                    Mes = DateTime.Now.Month.ToString("d2"),
//                    Ano = DateTime.Now.Day.ToString("d2"),
//                    Arquivos = arquivos
//                };

//                await _arquivoService.SalvarArquivo(arquivoDto);

//                var usuarioLog = _usuarioService.ConsumirSaldoExtrato(loginID, usuarioID, "Extrato Antigo", beneficio);
//                return file;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        [HttpGet("SolicitarIN100/{cpf}/{beneficio}")]
//        public async Task<Result<int>> SolicitarIN100(string cpf, string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.SaldoIN100 <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var numeroSolicitacao = await _service.SolicitarIN100Async(cpf, beneficio);

//                return Result<int>.Ok(numeroSolicitacao);
//            }
//            catch (Exception ex)
//            {
//                return Result<int>.Err(ex.Message);
//            }
//        }

//        [HttpGet("SolicitacaoIN100/{cpf}/{beneficio}")]
//        public async Task<Result<int>> SolicitacaoIN100(string cpf, string beneficio)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);

//                if (usuario.SaldoIN100 <= 0 && usuario.SaldoExtrato <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var creditoExtrato = usuario.SaldoIN100 <= 0 ? true : false;

//                var numeroSolicitacao = await _service.SolicitacaoIN100Async(cpf, beneficio, usuarioID, creditoExtrato);

//                if(usuario.SaldoIN100 <= 0)
//                    await _usuarioService.ConsumirSaldoIN100Extrato(loginID, usuarioID, "IN100/Extrato", beneficio);
//                else
//                    await _usuarioService.ConsumirSaldoIN100(loginID, usuarioID, "IN100", beneficio);

//                return Result<int>.Ok(numeroSolicitacao);
//            }
//            catch (Exception ex)
//            {
//                return Result<int>.Err(ex.Message);
//            }
//        }

//        [HttpGet("ObterIN100/{numeroSolicitacao}")]
//        public async Task<Result<PesquisaIN100DTO>> ObterIN100(string numeroSolicitacao)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);


//                if (usuario.SaldoIN100 <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var pesquisaIN100 = await _service.ObterIN100Async(numeroSolicitacao);

//                var resultadoTransformado = TransformaResultado(pesquisaIN100.Resultado);

//                var resultPesquisaIN100 = new PesquisaIN100DTO();
//                resultPesquisaIN100.CPF = pesquisaIN100.CPF;
//                resultPesquisaIN100.ErrorID = pesquisaIN100.ErrorID;
//                resultPesquisaIN100.NumeroBeneficio = pesquisaIN100.NumeroBeneficio;
//                resultPesquisaIN100.Situacao = pesquisaIN100.Situacao;
//                resultPesquisaIN100.Resultado = resultadoTransformado;

//                var usuarioLog = _usuarioService.ConsumirSaldoIN100(loginID, usuarioID, "IN100", pesquisaIN100.NumeroBeneficio);

//                return Result<PesquisaIN100DTO>.Ok(resultPesquisaIN100);

//            }
//            catch (Exception ex)
//            {
//                return Result<PesquisaIN100DTO>.Err(ex.Message);
//            }
//        }

//        [HttpGet("ObterIN100Usuario/{numeroSolicitacao}")]
//        public async Task<Result<PesquisaIN100DTO>> ObterIN100Usuario(string numeroSolicitacao)
//        {
//            try
//            {
//                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));
//                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
//                var usuario = await _usuarioService.GetById(usuarioID);


//                if (usuario.SaldoIN100 <= 0)
//                {
//                    throw new Exception("Saldo Insuficiente");
//                }

//                var pesquisaIN100 = await _service.ObterIN100UsuarioAsync(numeroSolicitacao);

//                var resultadoTransformado = TransformaResultado(pesquisaIN100.Resultado);

//                var resultPesquisaIN100 = new PesquisaIN100DTO();
//                resultPesquisaIN100.CPF = pesquisaIN100.CPF;
//                resultPesquisaIN100.ErrorID = pesquisaIN100.ErrorID;
//                resultPesquisaIN100.NumeroBeneficio = pesquisaIN100.NumeroBeneficio;
//                resultPesquisaIN100.Situacao = pesquisaIN100.Situacao;
//                resultPesquisaIN100.Resultado = resultadoTransformado;

//                var usuarioLog = _usuarioService.ConsumirSaldoIN100(loginID, usuarioID, "IN100", pesquisaIN100.NumeroBeneficio);

//                return Result<PesquisaIN100DTO>.Ok(resultPesquisaIN100);

//            }
//            catch (Exception ex)
//            {
//                return Result<PesquisaIN100DTO>.Err(ex.Message);
//            }
//        }

//        [HttpPost("ObterIN100Robo")]
//        public async Task<Result<bool>> ObterIN100Robo()
//        {
//            try
//            {
//                var result = await _service.ObterIN100Robo();;

//                return Result<bool>.Ok(result);

//            }
//            catch (Exception ex)
//            {
//                return Result<bool>.Err(ex.Message);
//            }
//        }

//        private int CalcularIdade(DateTime birthdate)
//        {
//            var today = DateTime.Now;
//            var age = today.Year - birthdate.Year;
//            if (birthdate > today.AddYears(-age))
//                age--;

//            return age;
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

//                    if(bancoLista != null)
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
//            if(resultado.DataNascimento != null)
//                idade = this.CalcularIdade((DateTime)resultado.DataNascimento);


//            var resp = new DadosIN100()
//            {
//                BeneficiarioID = resultado.BeneficiarioID == null ? 0 : resultado.BeneficiarioID,
//                Cpf = resultado.Cpf == null ? "" : resultado.Cpf,
//                NumeroBeneficio = resultado.NumeroBeneficio == null ? "" : resultado.NumeroBeneficio,
//                Nome = resultado.Nome == null ? "" : resultado.Nome,
//                DataNascimento = resultado.DataNascimento == null ? "" : (resultado.DataNascimento.Value.Day.ToString("D2") + "/" + resultado.DataNascimento.Value.Month.ToString("D2") + "/" + resultado.DataNascimento.Value.Year + $" ({idade} anos)"),
//                EmprestimoBloqueado = resultado.EmprestimoBloqueado == false ? resultado.PossuiRepresentanteLegal == true ? null : "NÃO" : "SIM",
//                EmprestimoElegivel =  resultado.PossuiRepresentanteLegal == true ? null : resultado.EmprestimoElegivel == false ? "NÃO" : "SIM",
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

//            if(resp.MensagemServidor.Contains("A requisição está sem o número do cpf do representante legal"))
//            {
//                resp.PossuiRepresentanteLegal = "SIM";
//                resp.PossuiProcurador = null;
//                resp.EmprestimoBloqueado = null;
//                resp.EmprestimoElegivel = null;
//                resp.MargemConsignavel = null;
//                resp.MargemConsignavelCartao = null;
//                resp.QtdEmprestimosAtivosSuspensos = null;
//            }

//            if (resp.MensagemServidor.Contains("estão bloqueados para empréstimo e"))
//            {
//                resp.PossuiRepresentanteLegal = null;
//                resp.PossuiProcurador = null;
//                resp.EmprestimoBloqueado = "SIM";
//                resp.EmprestimoElegivel = null;
//                resp.MargemConsignavel = null;
//                resp.MargemConsignavelCartao = null;
//                resp.QtdEmprestimosAtivosSuspensos = null;
//            }

//            return resp;

//        }


//    }
//}
