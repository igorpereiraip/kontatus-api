using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using Kontatus.Helper.Utilitarios;
using System;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface ILoginService : IService<Login>
    {
        Task<TokenConfig> Authenticate(string email, string senha);
        Task<TokenConfig> RefreshAuthenticate(int loginID);
        Task<TokenConfig> UpdateTimezone(int timezoneID, int loginID);
        Task<bool> Logout(string jwt);
    }

    public class LoginService : Service<Login>, ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly LoginDomainService _loginDomainService;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginService(
            ILoginRepository repository,
            ITokenRepository tokenRepository,
            ILoginRepository loginRepository,
            IUsuarioRepository usuarioRepository,
            LoginDomainService loginDomainService
            )
            : base(repository)
        {
            _loginDomainService = loginDomainService;
            _tokenRepository = tokenRepository;
            _loginRepository = loginRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<TokenConfig> Authenticate(string email, string senha)
        
        {
            TokenConfig tokenConfig = new TokenConfig();

            Login login = _loginRepository.Authenticate(email, senha).Result;
            var usuario = await _usuarioRepository.Obter(login.UsuarioID);
            var expiration = DateTime.UtcNow.AddDays(1);

            await _loginDomainService.RealizarLogin(tokenConfig, login, email, usuario, expiration);

            var token = new Token()
            {
                UsuarioID = usuario.ID,
                DataExpiracao = expiration,
                JWT = tokenConfig.Access,
            };

            await _tokenRepository.Create(token);

            return tokenConfig;
        }

        public async Task<bool> Logout(string jwt)
        {
            var token = await _tokenRepository.GetJWT(jwt);
            if(token != null)
                await _tokenRepository.Inactivate(token.ID);

            return true;
        }

        public async Task<TokenConfig> RefreshAuthenticate(int loginID)
        {
            TokenConfig tokenConfig = new TokenConfig();

            var login = await _loginRepository.RefreshAuthenticate(loginID);
            var usuario = await _usuarioRepository.Obter(login.UsuarioID);
            var expiration = DateTime.UtcNow.AddDays(1);

            await _loginDomainService.RealizarLogin(tokenConfig, login, login.Email, usuario, expiration);

            return tokenConfig;
        }

        public async Task<TokenConfig> UpdateTimezone(int timezoneID, int loginID)
        {
            var login = await _loginRepository.Get(loginID);

            await _loginRepository.Update(login);

            return await RefreshAuthenticate(loginID);
        }
    }
}
