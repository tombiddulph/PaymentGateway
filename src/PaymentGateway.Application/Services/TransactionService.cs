using System;
using System.Globalization;
using System.Threading.Tasks;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Application.Models;
using PaymentGateway.Models.Contracts;
using PaymentStatus = PaymentGateway.Models.Enums.PaymentStatus;
using Transaction = PaymentGateway.Models.Domain.Transaction;

namespace PaymentGateway.Application.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly IRepository<PaymentGateway.Application.Models.Transaction> _transactionRepo;

        public TransactionService(IRepository<Models.Transaction> transactionRepo)
        {
            _transactionRepo = transactionRepo ?? throw new ArgumentNullException(nameof(transactionRepo));
        }

        public async Task<Transaction> CreateTransactionAsync(CreatePaymentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }


            var transaction = MapToTransaction(request);
            var created = await _transactionRepo.AddAsync(transaction);

            return new Transaction
            {
                Id = created,
                Status = PaymentStatus.Success,
            };
        }

        public async Task<Transaction> GetById(Guid transactionId)
        {
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentException(nameof(transactionId));
            }

            var transaction = await _transactionRepo.GetByIdAsync(transactionId);

            if (transaction != null)
            {
                return new Transaction
                {
                    Id = transaction.Id,
                    Status = Enum.Parse<PaymentStatus>(transaction.Status.ToString("G")),
                    Amount = transaction.Amount,
                    CardNumber = transaction.Card.Number
                };
            }

            return new Transaction
            {
                Status = PaymentStatus.NotFound
            };
        }

        private static Models.Transaction MapToTransaction(CreatePaymentRequest request)
        {
            var cardId = Guid.NewGuid();
            
            return new Models.Transaction
            {
                Amount = decimal.Parse(request.Amount, NumberStyles.Currency, NumberFormatInfo.InvariantInfo),
                Status = Infrastructure.PaymentStatus.Success,
                Card = MapToCard(),
                CardId = cardId,
                Merchant = new Merchant
                {
                    Name = "Toms merchant"
                }
            };

            Card MapToCard()
            {
                return new Card
                {
                    Id = cardId,
                    Cvv = request.Cvv,
                    ExpiryMonth = request.ExpiryMonth,
                    ExpiryYear = request.ExpiryYear,
                    HolderName = request.Name,
                    Number = request.CardNumber
                };
            }
        }
    }

    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(CreatePaymentRequest request);
        Task<Transaction> GetById(Guid transactionId);
    }
}