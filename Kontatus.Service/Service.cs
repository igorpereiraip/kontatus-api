using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface IService<T> where T : Base
    {
        Task<IEnumerable<T>> List(bool basico = false);
        Task<T> GetById(int id);
        Task<T> Create(T entidade);
        Task<T> Update(T entidade);
        Task Delete(int id);
        Task Inactivate(int id);
        Task Activate(int id);
        Task<IEnumerable<T>> ListAll(bool basico = false);
        IQueryable<T> Find(Expression<Func<T, bool>> filter);
    }

    public class Service<T> : IService<T> where T : Base, new()
    {
        protected readonly IRepository<T> repository;

        public Service(IRepository<T> repository)
        {
            this.repository = repository;
        }

        public virtual async Task<T> Create(T entidade)
        {
            return await repository.Create(entidade);
        }

        public virtual async Task Delete(int id)
        {
            await repository.Delete(id);
        }

        public virtual async Task<IEnumerable<T>> List(bool basico = false)
        {
            return await repository.List();
        }
        public virtual async Task<IEnumerable<T>> ListAll(bool basico = false)
        {
            return await repository.ListAll();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await repository.Get(id);
        }

        public virtual async Task Inactivate(int id)
        {
            await repository.Inactivate(id);
        }

        public virtual async Task Activate(int id)
        {
            await repository.Activate(id);
        }

        public virtual async Task<T> Update(T entidade)
        {
            return await repository.Update(entidade);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> filter)
        {
            return repository.Find(filter);
        }
    }
}
