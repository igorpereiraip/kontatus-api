using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Helper.Utilitarios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsigIntegra.Data.Repository
{
    public interface ILoginRepository : IRepository<Login>
    {
        Task<Login> Authenticate(string email, string senha);
        Task<Login> RefreshAuthenticate(int loginID);
    }
    public class LoginRepository : Repository<Login>, ILoginRepository
    {
        public LoginRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {

        }

        public async Task<Login> Authenticate(string email, string senha)
        {
            Login login = await context.Logins.Where(x => x.Email == email && x.Senha == senha && x.Ativo).SingleOrDefaultAsync();

            if (login != null)             
                AtribuirAntigoLogin(new
                {
                    login.ID,
                    login.Email,
                    login.Principal,
                    login.Ativo,
                    login.TipoLoginNome,
                    DataLogin = DateTime.Now

                });

            return login;
        }

        public async Task<Login> RefreshAuthenticate(int loginID)
        {
            Login login = await context.Logins.Where(x => x.ID == loginID && x.Ativo).SingleOrDefaultAsync();

            AtribuirAntigoLogin(new
            {
                login.ID,
                login.Email,
                login.Principal,
                login.Ativo,
                login.TipoLoginNome,
                DataLogin = DateTime.Now

            });

            return login;
        }

        private void AtribuirAntigoLogin(object entidade)
        {
            _logUsuarioDTO.RegistroAntigo = Conversor.ConverterObjeto(entidade);
        }
    }
}
