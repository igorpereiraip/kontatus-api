using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IPessoaService
    {
        Task<bool> ExistePessoa(string cpf);
        Task<bool> CreateRange(List<Pessoa> listPessoa);
        Task<Pessoa> GetByCpf(string cpf);
        Task<bool> ValidatePessoaAsync(Pessoa pessoa, List<Pessoa> listPessoa);
    }

    public class PessoaService : Service<Pessoa>, IPessoaService
    {
        private readonly IPessoaRepository repository;
        public PessoaService(IPessoaRepository repository)
            : base()
        {
            this.repository = repository;
        }

        public async Task<bool> ValidatePessoaAsync(Pessoa pessoa, List<Pessoa> listPessoa)
        {
            return !String.IsNullOrEmpty(pessoa.CPF) && !String.IsNullOrEmpty(pessoa.Nome) &&
                (listPessoa.Find(z=> z.CPF == pessoa.CPF) == null) && 
                !await repository.Find(x=> x.CPF == pessoa.CPF && x.Ativo).AnyAsync();
        }

        public async Task<bool> ExistePessoa(string cpf)
        {
            return await repository.Find(x => x.CPF == cpf).AnyAsync();
        }

        public async Task<bool> CreateRange(List<Pessoa> listPessoa)
        {
            await repository.CreateRange(listPessoa);
            return true;
        }

        public async Task<Pessoa> GetByCpf(string cpf)
        {
            return await repository.Find(x => x.CPF == cpf && x.Ativo).FirstOrDefaultAsync() ;
        }
    }
}
