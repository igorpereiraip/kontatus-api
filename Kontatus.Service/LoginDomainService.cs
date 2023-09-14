using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Kontatus.Helper.Utilitarios;
using Kontatus.Domain.Entity;
using Kontatus.Data.Repository;
using System.Text.Json;

namespace Kontatus.Service
{
    public class LoginDomainService
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginDomainService(IConfiguration config, IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _usuarioRepository = usuarioRepository;
        }

        public async Task RealizarLogin(TokenConfig tokenConfig, Login login, string email, Usuario usuario, DateTime expiration)
        {
            if (login != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, email),
                    new Claim("login", JsonSerializer.Serialize(login)),
                    new Claim("user", JsonSerializer.Serialize(usuario)),
                    new Claim("exp", JwtRegisteredClaimNames.Exp, ((DateTimeOffset)expiration).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").ToString()));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


                JwtSecurityToken token = new JwtSecurityToken(
                   issuer: null,
                   audience: null,
                   claims: claims,
                   expires: expiration,
                   signingCredentials: creds);


                tokenConfig.Access = new JwtSecurityTokenHandler().WriteToken(token);
                tokenConfig.Refresh = new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
