using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Helper.Utilitarios;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
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

        public LoginService(
            ILoginRepository repository,
            ITokenRepository tokenRepository,
            ILoginRepository loginRepository,
            LoginDomainService loginDomainService
            )
            : base(repository)
        {
            _loginDomainService = loginDomainService;
            _tokenRepository = tokenRepository;
            _loginRepository = loginRepository;
        }

        public async Task<TokenConfig> Authenticate(string email, string senha)
        
        {
            TokenConfig tokenConfig = new TokenConfig();

            Login login = _loginRepository.Authenticate(email, senha).Result;

            await _loginDomainService.RealizarLogin(tokenConfig, login, email);

            var token = new Token()
            {
                UsuarioID = tokenConfig.Usuario.ID,
                DataExpiracao = tokenConfig.Expiration,
                JWT = tokenConfig.JWT,
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

            await _loginDomainService.RealizarLogin(tokenConfig, login, login.Email);

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
