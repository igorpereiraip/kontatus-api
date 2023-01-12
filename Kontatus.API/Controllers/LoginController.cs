
using AutoMapper;
using Kontatus.API.Configurations;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Kontatus.Domain.ViewModels;
using Kontatus.Helper.Utilitarios;
using Kontatus.Service;
using Kontatus.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kontatus.API.Controllers
{
    [ClaimRequirement(ClaimTypes.Name, "Authorization")]
    public class LoginController : Controller<Login>
    {
        private readonly IMapper mapper;

        public LoginController(ILoginService service, IMapper mapper)
            : base(service)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Cria um novo registro com os dados informados
        /// </summary>
        /// <returns>Registro criado contendo o Id</returns>
        [HttpPost]
        public override async Task<Result<Login>> Create([FromBody] Login login)
        {
            try
            {
                login.Senha = login.Senha;

                return Result<Login>.Ok(await service.Create(login));
            }
            catch (System.Exception ex)
            {
                return Result<Login>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Autentica o usuário através do e-mail e senha
        /// </summary>
        /// <returns>Token de acesso às APIs</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<Result<TokenConfigViewModel>> Authenticate([FromBody] LoginAutenticacao login)
        {
            try
            {
                var tokenConfig = mapper.Map<TokenConfigViewModel>(await (service as ILoginService).Authenticate(login.Email, login.Senha));

                if (string.IsNullOrEmpty(tokenConfig.JWT))
                    return Result<TokenConfigViewModel>.Err("As credenciais informadas não são válidas");

                return Result<TokenConfigViewModel>.Ok(tokenConfig);
            }
            catch (System.Exception ex)
            {
                return Result<TokenConfigViewModel>.Err(ex.Message);
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Logout</returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<Result<bool>> Logout()
        {
            try
            {
                var jwt = Request.HttpContext.Request.Headers.ExtractJwtFromHeader()?.RawData;
                await (service as ILoginService).Logout(jwt);


                return Result<bool>.Ok(true);
            }
            catch (System.Exception ex)
            {
                return Result<bool>.Err(ex.Message);
            }
        }

        ///// <summary>
        ///// Atualiza o token do usuário antes que expire
        ///// </summary>
        ///// <returns>Token de acesso às APIs</returns>
        //[HttpPut]
        //[Route("refresh-authenticate")]
        //public async Task<Result<TokenConfigViewModel>> RefreshToken()
        //{
        //    try
        //    {
        //        var loginID = int.Parse(Request.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
        //        var tagUrl = Request.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("tagUrlCliente");

        //        var tokenConfig = mapper.Map<TokenConfigViewModel>(await (service as ILoginService).RefreshAuthenticate(loginID));

        //        if (string.IsNullOrEmpty(tokenConfig.JWT))
        //            return Result<TokenConfigViewModel>.Err("As credenciais informadas não são válidas");


        //        return Result<TokenConfigViewModel>.Ok(tokenConfig);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<TokenConfigViewModel>.Err(ex.Message);
        //    }
        //}

        ///// <summary>
        ///// Atualiza o timezone do login do usuario
        ///// </summary>
        ///// <returns>Token de acesso às APIs</returns>
        //[HttpPut("alterar-timezone/{timezoneID}")]
        //public virtual async Task<Result<TokenConfigViewModel>> AlterarTimezone(int timezoneID)
        //{
        //    try
        //    {
        //        var loginID = int.Parse(Request.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("loginID"));
        //        var tagUrl = Request.HttpContext.Request.Headers.ExtractJwtFromHeader()?.GetClaimFromJWT("tagUrlCliente");

        //        var tokenConfig = mapper.Map<TokenConfigViewModel>(await (service as ILoginService).UpdateTimezone(timezoneID, loginID));

        //        if (string.IsNullOrEmpty(tokenConfig.JWT))
        //            return Result<TokenConfigViewModel>.Err("As credenciais informadas não são válidas");

        //        return Result<TokenConfigViewModel>.Ok(tokenConfig);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<TokenConfigViewModel>.Err(ex.Message);
        //    }
        //}
    }
}
