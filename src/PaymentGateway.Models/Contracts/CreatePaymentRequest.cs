using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models.Contracts
{
    public class CreatePaymentRequest
    {
        [Required, StringLength(maximumLength: 16, MinimumLength = 16)]
        public string CardNumber { get; set; }

        [Required, StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpiryMonth { get; set; }

        [Required, StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpiryYear { get; set; }

        [Required, RegularExpression(@"[0-9]{0,5}\.[0-9]{2}")]
        public string Amount { get; set; }

        [Required, StringLength(maximumLength: 4, MinimumLength = 3), RegularExpression(@"\d")]
        public string Cvv { get; set; }
    }
}