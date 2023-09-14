using Kontatus.Data.Repository;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface ITelefoneService
    {
        Task<bool> ValidateTelefoneFullAsync(Telefone telefone, List<Telefone> listTelefone);
        Task<bool> CreateRange(List<Telefone> telefones);
    }

    public class TelefoneService : Service<Telefone>, ITelefoneService
    {
        private readonly ITelefoneRepository repository;
        public TelefoneService(ITelefoneRepository repository)
            : base()
        {
            this.repository = repository;
        }

        public async Task<bool> ValidateTelefoneFullAsync(Telefone telefone, List<Telefone> listTelefone)
        {
            return !String.IsNullOrEmpty(telefone.NumeroTelefone) &&
                listTelefone.Find(z=> z.PessoaId == telefone.PessoaId && z.NumeroTelefone == telefone.NumeroTelefone) == null &&
                !(await repository.Find(x => x.NumeroTelefone == telefone.NumeroTelefone && x.PessoaId == telefone.PessoaId).AnyAsync());
        }

        public async Task<bool> CreateRange(List<Telefone> telefones)
        {
            await repository.CreateRange(telefones);
            return true;
        }

    }
}
