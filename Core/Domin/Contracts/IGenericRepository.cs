using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(object id);

        Task<T?> GetWithSpecAsync(ISpecifications<T> spec);

        Task AddAsync(T entity);

        void Update(T entity);
        void Delete(T entity);
    }
}
