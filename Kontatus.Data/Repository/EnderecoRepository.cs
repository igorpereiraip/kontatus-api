using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
    }
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        //public async Task<int> Teste()
        //{

        //    var x = 0;

        //    var list = new List<Pessoa>();

        //    while (x < 100000)
        //    {
        //        var pessoa = new Pessoa
        //        {
        //            Nome = "Igor Teste",
        //            CPF = "09342878997",
        //            DataNascimento = "07/01/1995",
        //            Idade = x,
        //            Aposentado = true
        //        };

        //        list.Add(pessoa);


        //        x++;
        //    }
        //    await context.Pessoas.AddRangeAsync(list);

        //    await context.SaveChangesAsync();



        //    return x;
        //}
    }
}
