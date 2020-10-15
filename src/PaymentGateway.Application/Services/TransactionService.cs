using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Application.Infrastructure;
using PaymentGateway.Application.Models;
using PaymentGateway.Models.Contracts;
using PaymentStatus = PaymentGateway.Models.Enums.PaymentStatus;
using DomainTransaction = PaymentGateway.Models.Domain.Transaction;

namespace PaymentGateway.Application.Services
{
    internal class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> _transactionRepo;

        private static readonly DomainTransaction NotFound = new DomainTransaction
        {
            Status = PaymentStatus.NotFound
        };

        public TransactionService(IRepository<Transaction> transactionRepo)
        {
            _transactionRepo = transactionRepo ?? throw new ArgumentNullException(nameof(transactionRepo));
        }

        public Task<DomainTransaction> CreateTransactionAsync(CreatePaymentRequest request, string userId)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            async Task<DomainTransaction> Create()
            {
                var transaction = MapToTransaction(request);
                transaction.UserId = userId;
                var created = await _transactionRepo.AddAsync(transaction);

                return new DomainTransaction
                {
                    Id = created,
                    Status = PaymentStatus.Success,
                };
            }

            return Create();
        }


        public Task<DomainTransaction> GetById(Guid transactionId, string userId)
        {
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentException(nameof(transactionId));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return Get();

            async Task<DomainTransaction> Get()
            {
                var transaction = await _transactionRepo.FindAsync(x => x.Id == transactionId && x.UserId == userId);

                if (transaction is {})
                {
                    return new DomainTransaction
                    {
                        Id = transaction.Id,
                        Status = Enum.Parse<PaymentStatus>(transaction.Status.ToString("G")),
                        Amount = transaction.Amount,
                        CardNumber = transaction.Card.MaskCardNumber()
                    };
                }

                return NotFound;
            }
        }

        private static Transaction MapToTransaction(CreatePaymentRequest request)
        {
            var cardId = Guid.NewGuid();

            return new Transaction
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
        Task<DomainTransaction> CreateTransactionAsync(CreatePaymentRequest request, string userId);
        Task<DomainTransaction> GetById(Guid transactionId, string userId);
    }
}