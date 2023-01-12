using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ConsigIntegra.Helper.Utilitarios;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Data.Repository;

namespace ConsigIntegra.Service
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

        public async Task RealizarLogin(TokenConfig tokenConfig, Login login, string email)
        {
            if (login != null)
            {
                var usuario = await _usuarioRepository.Obter(login.UsuarioID);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, email),
                    new Claim("loginID", login.ID.ToString()),
                    new Claim("usuarioID", login.UsuarioID.ToString()),
                    new Claim("administrador", usuario.Administrador.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").ToString()));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddDays(1);
                JwtSecurityToken token = new JwtSecurityToken(
                   issuer: null,
                   audience: null,
                   claims: claims,
                   expires: expiration,
                   signingCredentials: creds);


                tokenConfig.JWT = new JwtSecurityTokenHandler().WriteToken(token);
                tokenConfig.Expiration = expiration;
                tokenConfig.Login = login;
                tokenConfig.Usuario = usuario;
            }
        }
    }
}
