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
using Microsoft.Graph;
using System.Text.Json;

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
