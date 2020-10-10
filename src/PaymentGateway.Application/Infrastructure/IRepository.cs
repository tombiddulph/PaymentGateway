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

    public class Repository<T> : IRepository<T> where T : class, IModelId
    {
        private readonly GatewayDbContext _context;

        public Repository(GatewayDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<Guid> AddAsync(T entity)
        {
            var added = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return added.Entity.Id;
        }
    }
}