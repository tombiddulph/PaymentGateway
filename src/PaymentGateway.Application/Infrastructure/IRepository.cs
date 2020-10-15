using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public interface IRepository<T> where T : class, IModelId
    {
        Task<T> FindAsync(Expression<Func<T, bool>> selector);
        Task<T> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(T entity);
    }
}