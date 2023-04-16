using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IEnderecoService : IService<Endereco>
    {
    }

    public class EnderecoService : Service<Endereco>, IEnderecoService
    {
        public EnderecoService(IEnderecoRepository repository)
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
