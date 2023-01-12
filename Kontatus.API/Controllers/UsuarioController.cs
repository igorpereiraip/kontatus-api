using Kontatus.API.Configurations;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Kontatus.Domain.Enums;
using Kontatus.Helper.Utilitarios;
using Kontatus.Service;
using Kontatus.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kontatus.API.Controllers
{
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    public class UsuarioController : Controller<Usuario>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoginService loginService;
        public UsuarioController(IUsuarioService service, IHttpContextAccessor httpContextAccessor, ILoginService loginService)
            : base(service)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.loginService = loginService;
        }
        /// <summary>
        /// Cria um novo registro com os dados informados
        /// </summary>
        /// <returns>Registro criado contendo o Id</returns>
        [HttpPost("incluir")]
        [AllowAnonymous]
        public virtual async Task<Result<Usuario>> Incluir([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var usuarioCriado = await (service as UsuarioService).Create(usuario);

                if (usuarioCriado == null)
                    throw new Exception("Erro ao criar Usuário");

                return Result<Usuario>.Ok(usuarioCriado);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Retorna os dados do Usuário da Requisição
        /// </summary>
        /// <returns>Registro do Usuário vinculado ao Token da Requisição</returns>
        [HttpGet("getbytoken")]
        public virtual async Task<Result<TokenConfig>> GetByToken()
        {
            try
            {
                TokenConfig result;
                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
                var expiration = httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader().ValidTo;

                //if ((expiration - DateTime.UtcNow).Minutes <= 10)
                //{
                //    //result = await loginService.RefreshAuthenticate(loginID);
                //}
                //else
                //{
                    var login = await loginService.GetById(loginID);
                    var usuario = await (service as IUsuarioService).GetByLoginID(loginID);
                    result = new TokenConfig
                    {
                        Usuario = usuario,
                        Login = login,
                        JWT = httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader().RawData,
                        Expiration = httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader().ValidTo,
                    };
                //}

                return Result<TokenConfig>.Ok(result);
            }
            catch (Exception ex)
            {
                return Result<TokenConfig>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um registro de acordo com o id e dados informados
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("alterar/{id}")]
        public virtual async Task<Result<Usuario>> Alterar(int id, [FromBody] UsuarioDTO usuario)
        {
            try
            {
                var usuarioAlterado = await (service as UsuarioService).Update(id, usuario);

                if (usuarioAlterado == null)
                    throw new Exception("Erro ao alterar Usuário");

                return Result<Usuario>.Ok(usuarioAlterado);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Altera saldo IN100 para usuario
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("adicionarSaldoIN100/{id}/{valor}")]
        public virtual async Task<Result<Usuario>> AdicionarSaldoIN100(int id, int valor)
        {
            try
            {
                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
                var usuarioAlterado = await (service as UsuarioService).AdicionarSaldoIN100(loginID, id, valor);

                if (usuarioAlterado == null)
                    throw new Exception("Erro ao alterar Usuário");

                return Result<Usuario>.Ok(usuarioAlterado);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Altera saldo Extrato para usuario
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("adicionarSaldoExtrato/{id}/{valor}")]
        public virtual async Task<Result<Usuario>> AdicionarSaldoExtrato(int id, int valor)
        {
            try
            {
                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
                var usuarioAlterado = await (service as UsuarioService).AdicionarSaldoExtrato(loginID, id, valor);

                if (usuarioAlterado == null)
                    throw new Exception("Erro ao alterar Usuário");

                return Result<Usuario>.Ok(usuarioAlterado);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Altera saldo Offline para usuario
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("adicionarSaldoOffline/{id}/{valor}")]
        public virtual async Task<Result<Usuario>> AdicionarSaldoOffline(int id, int valor)
        {
            try
            {
                var loginID = int.Parse(httpContextAccessor.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
                var usuarioAlterado = await (service as UsuarioService).AdicionarSaldoOffline(loginID, id, valor);

                if (usuarioAlterado == null)
                    throw new Exception("Erro ao alterar Usuário");

                return Result<Usuario>.Ok(usuarioAlterado);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Inativa um usuário de acordo com o id e dados informados
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("inativar/{id}")]
        public virtual Result<object> Inativar(int id)
        {
            try
            {
                service.Inactivate(id);

                return Result<object>.Ok();
            }
            catch (Exception ex)
            {
                return Result<object>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Reativa um usuário de acordo com o id e dados informados
        /// </summary>
        /// <returns>Registro atualizado</returns>
        [HttpPut("ativar/{id}")]
        public virtual Result<object> Ativar(int id)
        {
            try
            {
                service.Activate(id).Wait();

                return Result<object>.Ok();
            }
            catch (Exception ex)
            {
                return Result<object>.Err(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Retorna o registro de acordo com o Id
        /// </summary>
        /// <returns>Registro com o id informado</returns>
        [HttpGet("getbyid/{id}")]
        public async Task<Result<UsuarioDTO>> GetById(int id)
        {
            try
            {
                var usuarioDTO = new UsuarioDTO();

                var usuario = await (service as UsuarioService).GetById(id);

                if (usuario != null)
                {
                    usuarioDTO.Administrador = usuario.Administrador;
                    usuarioDTO.Email = usuario.Logins.First().Email;
                    usuarioDTO.ID = usuario.ID;
                    usuarioDTO.Nome = usuario.Nome;
                    usuarioDTO.SaldoIN100 = usuario.SaldoIN100;
                    usuarioDTO.SaldoExtrato = usuario.SaldoExtrato;
                    usuarioDTO.SaldoOffline = usuario.SaldoOffline;
                    usuarioDTO.OfflineIlimitado = usuario.OfflineIlimitado;
                    usuarioDTO.LimiteDiario = usuario.LimiteDiario;
                    usuarioDTO.Senha = usuario.Logins.First().Senha;
                    usuarioDTO.AcessosSimultaneos = usuario.AcessosSimultaneos;
                    usuarioDTO.ValidadePlano = usuario.ValidadePlano;
                }

                return Result<UsuarioDTO>.Ok(usuarioDTO);
            }
            catch (Exception ex)
            {
                return Result<UsuarioDTO>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza a Senha do Usuário enviado
        /// </summary>
        /// <param name="usuario">Objeto do Tipo Usuário</param>
        /// <returns>Atualiza a Senha do Usuário enviado</returns>
        [HttpPut("updatePassword")]
        [AllowAnonymous]
        public virtual async Task<Result<object>> UpdatePassword([FromBody] UsuarioDTO usuario)
        {
            try
            {
                await (service as IUsuarioService).UpdatePassword(usuario.Email);

                return Result<object>.Ok();
            }
            catch (Exception ex)
            {
                return Result<object>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza para senha escolhida pelo usuário
        /// </summary>
        /// <param name="usuario">Objeto do Tipo Usuário</param>
        /// <returns>Atualiza para senha escolhida pelo usuário</returns>
        [HttpPut("AlterarSenha")]
        public async Task<Result<object>> AlterarSenha([FromBody] UsuarioDTO usuario)
        {
            try
            {
                var usuarioID = int.Parse(HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("usuarioID"));

                await (service as IUsuarioService).AlterarSenha(usuario.Email, usuario.Senha, usuarioID);

                return Result<object>.Ok();
            }
            catch (Exception ex)
            {
                return Result<object>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Retorna os Usuários
        /// </summary>
        /// <returns>Lista de Todos os Usuários</returns>
        [HttpGet("getUsuarios")]
        public virtual async Task<Result<List<Usuario>>> GetUsuarios()
        {
            try
            {
                var usuarios = await (service as IUsuarioService).GetUsuarios();

                return Result<List<Usuario>>.Ok(usuarios);
            }
            catch (Exception ex)
            {
                return Result<List<Usuario>>.Err(ex.Message);
            }


        }
    }
}
