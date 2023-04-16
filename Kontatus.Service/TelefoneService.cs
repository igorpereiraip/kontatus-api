using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface ITelefoneService : IService<Telefone>
    {
    }

    public class TelefoneService : Service<Telefone>, ITelefoneService
    {
        public TelefoneService(ITelefoneRepository repository)
            : base(repository)
        {
        }

        //public async Task<int> Teste()
        //{
        //    var number = await (repository as PessoaRepository).Teste();


        //    return number;
        //}
    }
}
