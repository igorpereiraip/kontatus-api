using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<bool> CreateRange(List<Pessoa> listPessoa);
        Task<List<Pessoa>> GetPersonByName(string name, string uf, string year);
        Task<List<Pessoa>> GetPersonByCpf(string cpf);
    }
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<bool> CreateRange(List<Pessoa> listPessoa)
        {
            await context.Pessoas.AddRangeAsync(listPessoa);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Pessoa>> GetPersonByName(string name, string uf, string year)
        {
            var personList = await context.Pessoas.Where(x=> x.Nome.Contains(name)).Include(x=> x.Telefones).Include(x=> x.Emails).Include(x=> x.Enderecos).ToListAsync();

            if (!string.IsNullOrEmpty(uf) && personList.Count > 0)
                personList = personList.Where(x => x.Enderecos.Where(y => y.Uf == uf).Any()).ToList();
            if (!string.IsNullOrEmpty(year) && personList.Count > 0)
                personList = personList.Where(x=> DateTime.Parse(x.DataNascimento).Year.ToString().Contains(year)).ToList();

            return personList;
        }

        public async Task<List<Pessoa>> GetPersonByCpf(string cpf)
        {
            var personList = await context.Pessoas.Where(x => x.CPF.Contains(cpf)).Include(x => x.Telefones).Include(x => x.Emails).Include(x => x.Enderecos).ToListAsync();
            return personList;
        }

    }
}
