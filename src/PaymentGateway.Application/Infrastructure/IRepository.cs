using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public interface IRepository<T> where T : class, IModelId
    {
        Task<T> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(T entity);
    }


    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly GatewayDbContext _context;

        public TransactionRepository(GatewayDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            return transaction;
        }

        public async Task<Guid> AddAsync(Transaction entity)
        {
            var t = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return t.Entity.Id;
        }
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
            try
            {
                var added = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return added.Entity.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}