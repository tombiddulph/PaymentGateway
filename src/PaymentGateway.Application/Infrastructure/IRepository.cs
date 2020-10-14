using System;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public interface IRepository<T> where T : class, IModelId
    {
        Task<T> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(T entity);
    }
}