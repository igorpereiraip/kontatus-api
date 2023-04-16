using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IPessoaService : IService<Pessoa>
    {
        Task<int> Teste();
    }

    public class PessoaService : Service<Pessoa>, IPessoaService
    {
        public PessoaService(IPessoaRepository repository)
            : base(repository)
        {
        }

        public async Task<int> Teste()
        {
            var number = await (repository as PessoaRepository).Teste();


            return number;
        }
    }
}
