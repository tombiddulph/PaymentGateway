using System.Threading.Tasks;

namespace PaymentGateway.Application.Services
{
    /// <summary>
    /// This mock payment validation service could be switched out to a real bank at a later point
    /// </summary>
    public class MockPaymentValidationService : IPaymentValidator
    {
       
        public Task<bool> ValidateAsync(Payment payment)
        {
            return Task.FromResult(true);
        }
    }


    /// <summary>
    /// This interface provides a method to validate a given payment
    /// </summary>
    public interface IPaymentValidator
    {
        Task<bool>  ValidateAsync(Payment payment);
    }

    public class Payment
    {
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        
    }
}