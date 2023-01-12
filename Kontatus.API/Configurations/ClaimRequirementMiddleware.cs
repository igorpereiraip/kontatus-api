using ConsigIntegra.Data.Repository;
using ConsigIntegra.Helper.Utilitarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ConsigIntegra.API.Configurations
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }

    public class ClaimRequirementFilter : IAsyncAuthorizationFilter
    {
        readonly Claim _claim;
        private readonly ITokenRepository _tokenRepository;

        public ClaimRequirementFilter(Claim claim, ITokenRepository tokenRepository)
        {
            _claim = claim;
            _tokenRepository = tokenRepository;
        }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var auth = context.ActionDescriptor.RouteValues.Values.Contains("Authenticate");
            var logout = context.ActionDescriptor.RouteValues.Values.Contains("Logout");
            var jwtSession = context.HttpContext.Request.Headers.ExtractJwtFromHeader()?.RawData;
            if (!auth && !logout && jwtSession != null)
            {
                var jwt = await _tokenRepository.GetJWT(jwtSession);
                if (jwt == null || !jwt.Ativo || DateTime.Now > jwt.DataExpiracao)
                {
                    context.Result = new UnauthorizedResult();
                }

                if(jwt != null)
                {
                    var userJWT = await _tokenRepository.GetTokensAtivosPorUsuario(jwt.UsuarioID);
                    var listaInactivate = userJWT.Skip((int)userJWT.First().Usuario.AcessosSimultaneos);
                    foreach (var token in listaInactivate)
                    {
                        await _tokenRepository.Inactivate(token.ID);
                    }
                }


            }
        }
    }
}
