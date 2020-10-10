using System;
using PaymentGateway.Models.Domain;
using PaymentGateway.Models.Enums;

namespace PaymentGateway.Application.Services
{
    internal class TransactionService : ITransactionService
    {
        public Transaction CreateTransaction(object request)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Status = PaymentStatus.Success
            };
        }

        public Transaction GetById(Guid transactionId)
        {
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentException(nameof(transactionId));
            }

            return new Transaction
            {
                Id = transactionId,
                Status = PaymentStatus.Success
            };
        }
    }

    public interface ITransactionService
    {
        Transaction CreateTransaction(object request);
        Transaction GetById(Guid transactionId);
    }
}