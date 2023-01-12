using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IRepository<T> where T : Base, new()
    {
        Task<IEnumerable<T>> List();
        Task<T> Get(int id);
        Task<T> Create(T entidade);
        Task<T> Update(T entidade);
        Task Delete(int id);
        Task Inactivate(int id);
        Task Activate(int id);
        Task<IEnumerable<T>> ListAll();
        IQueryable<T> Find(Expression<Func<T, bool>> filter);
        IQueryable<T> Paginar(IQueryable<T> query, int? pagina = null, int? qtde = null);
    }

    public class Repository<T> : IRepository<T> where T : Base, new()
    {
        protected readonly KontatusContext context;
        protected readonly LogUsuarioDTO _logUsuarioDTO;
        protected string[] incluir;

        public Repository(KontatusContext context, LogUsuarioDTO logUsuarioDTO)
        {
            this.context = context;
            _logUsuarioDTO = logUsuarioDTO;
        }

        public virtual async Task<IEnumerable<T>> List()
        {
            IQueryable<T> query = context.Set<T>();

            if (incluir != null)
            {
                foreach (var item in incluir)
                {
                    query = query.Include(item);
                }
            }

            query = query.Where(x => x.Ativo == true);

            return await query.ToListAsync();
        }
        public virtual async Task<IEnumerable<T>> ListAll()
        {
            IQueryable<T> query = context.Set<T>();

            return await query.ToListAsync();
        }

        public virtual async Task<T> Get(int id)
        {
            IQueryable<T> query = context.Set<T>();

            if (incluir != null)
            {
                foreach (var item in incluir)
                {
                    query = query.Include(item);
                }
            }

            query = query.Where(x => x.Ativo);

            var entidade = await query.SingleOrDefaultAsync(x => x.ID == id);
            if (entidade is null)
            {
                throw new ValidationException($"{typeof(T).Name} com id: {id} não encontrado");
            }
            return entidade;
        }

        public virtual async Task<T> GetAll(int id)
        {
            IQueryable<T> query = context.Set<T>();

            var entidade = await query.SingleOrDefaultAsync(x => x.ID == id);
            if (entidade is null)
            {
                throw new ValidationException($"{typeof(T).Name} com id: {id} não encontrado");
            }
            return entidade;
        }

        public virtual async Task<T> Create(T entidade)
        {
            context.Entry(entidade).State = EntityState.Added;

            await Salvar();

            return entidade;
        }

        public virtual async Task<T> Update(T entidade)
        {
            entidade.DataAlteracao = DateTime.Now;

            context.Entry(entidade).State = EntityState.Modified;

            await Salvar();

            return entidade;
        }

        public virtual async Task Delete(int id)
        {
            var entidade = context.Set<T>().Find(id);
            context.Entry(entidade).State = EntityState.Deleted;
            await Salvar();
        }

        public virtual async Task Inactivate(int id)
        {
            var entidade = context.Set<T>().Find(id);
            entidade.DataAlteracao = DateTime.Now;
            entidade.Ativo = false;
            context.Entry(entidade).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public virtual async Task Activate(int id)
        {
            var entidade = await GetAll(id);
            entidade.DataAlteracao = DateTime.Now;
            entidade.Ativo = true;
            context.Entry(entidade).State = EntityState.Modified;
            await Salvar();
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> filtro)
        {
            var query = context.Set<T>().Where(filtro).Where(x => x.Ativo);

            query = query.Where(x => x.Ativo == true);

            return query;
        }

        public virtual IQueryable<T> Paginar(IQueryable<T> query, int? pagina = null, int? qtde = null)
        {
            if (pagina != null && qtde != null)
            {
                if (pagina == 0)
                {
                    pagina = 1;
                }
                query = query.Skip(((int)pagina - 1) * (int)qtde).Take((int)qtde);
            }

            return query;
        }

        protected async Task Salvar()
        {
            await context.SaveChangesAsync();
        }
    }
}