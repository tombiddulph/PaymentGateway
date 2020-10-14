using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class, IModelId
    {
        private readonly PaymentGatewayDbContext _context;
        private readonly ILogger<Repository<T>> _logger;
        private readonly string _typeName = typeof(T).Name;

        public Repository(PaymentGatewayDbContext context, ILogger<Repository<T>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<T> GetByIdAsync(Guid id)
        {
            Func<Guid, Task<T>> func = async guid =>
            {
                var entity = await _context.Set<T>().FindAsync(id);

                if (entity != null)
                {
                    return entity;
                }

                _logger.LogDebug($"{_typeName} with id {id} not found");

                return null;
            };

            return await func.SafeInvoke(OnError, id);
        }

        public async Task<Guid> AddAsync(T entity)
        {
            Func<T, Task<Guid>> func = async t =>
            {
                var added = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return added.Entity.Id;
            };

            return await func.SafeInvoke(OnError, entity);
        }

        private void OnError(Exception e)
        {
            _logger.LogError($"An error occured processing {_typeName}", e);
        }
    }
}