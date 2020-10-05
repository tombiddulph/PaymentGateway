using System;
using PaymentGateway.Models.Domain;

namespace PaymentGateway.Application.Services
{
    internal class TransactionService : ITransactionService
    {
        public Transaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public Transaction GetById(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }

    public interface ITransactionService
    {
        Transaction CreateTransaction();
        Transaction GetById(Guid transactionId);
    }
}