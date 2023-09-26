using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IEnderecoService : IService<Endereco>
	{
        Task<bool> ValidateEnderecoAsync(Endereco endereco, List<Endereco> listEndereco);
        Task<bool> CreateRange(List<Endereco> enderecos);
    }

    public class EnderecoService : Service<Endereco>, IEnderecoService
    {
        private readonly IEnderecoRepository repository;
        public EnderecoService(IEnderecoRepository repository)
            : base()
        {
            this.repository = repository;
        }

        public async Task<bool> ValidateEnderecoAsync(Endereco endereco, List<Endereco> listEndereco)
        {
            return !String.IsNullOrEmpty(endereco.DescricaoEndereco) &&
                listEndereco.Find(z=> z.PessoaId == endereco.PessoaId && z.DescricaoEndereco == endereco.DescricaoEndereco) == null &&
                !await repository.Find(x=> x.PessoaId == endereco.PessoaId && x.DescricaoEndereco == endereco.DescricaoEndereco).AnyAsync();
        }

        public async Task<bool> CreateRange(List<Endereco> enderecos)
        {
            await repository.CreateRange(enderecos);
            return true;
        }
    }
}
