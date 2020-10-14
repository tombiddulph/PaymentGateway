using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PaymentGateway.Api;

namespace PaymentGateway.Models.Contracts
{
    public class CreatePaymentRequest : IValidatableObject
    {
        [Required, StringLength(maximumLength: 16, MinimumLength = 15),
         RegularExpression(@"\d{15,16}", ErrorMessage = RequestErrors.Numeric)]
        public string CardNumber { get; set; }

        [Required, StringLength(maximumLength: 2, MinimumLength = 2),
         RegularExpression(@"\d{2}", ErrorMessage = RequestErrors.Numeric)]
        public string ExpiryMonth { get; set; }

        [Required, StringLength(maximumLength: 2, MinimumLength = 2),
         RegularExpression(@"\d{2}", ErrorMessage = RequestErrors.Numeric)]
        public string ExpiryYear { get; set; }

        [Required, RegularExpression(@"[0-9]{0,5}\.[0-9]{2}", ErrorMessage = RequestErrors.Amount)]
        public string Amount { get; set; }

        [Required, StringLength(maximumLength: 4, MinimumLength = 3, ErrorMessage = RequestErrors.Cvv),
         RegularExpression(@"\d{3,4}", ErrorMessage = RequestErrors.Numeric)]
        public string Cvv { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var isValidExpiry = DateTime.TryParseExact($"{ExpiryMonth}{ExpiryYear}", "MMyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsedDate);

            if (!isValidExpiry)
            {
                yield return new ValidationResult("The computed card expiry date is invalid", new[]
                {
                    nameof(ExpiryMonth),
                    nameof(ExpiryYear)
                });
            }

            var now = DateTimeProvider.Now();
            if (parsedDate.Year < now.Year)
            {
                yield return new ValidationResult("The expiry year is in the past", new[]
                {
                    nameof(ExpiryYear)
                });
            }

            if (parsedDate.Year == now.Year && parsedDate.Month < now.Month)
            {
                yield return new ValidationResult("The computed expiry date is in the past", new[]
                {
                    nameof(ExpiryMonth),
                    nameof(ExpiryYear)
                });
            }

            var isValidAmount = decimal.TryParse(Amount, NumberStyles.Currency, CultureInfo.InvariantCulture,
                out var parsedAmount);


            if (!isValidAmount)
            {
                yield return new ValidationResult("The field is invalid", new[] { nameof(Amount) });
            }

            if (parsedAmount <= 0)
            {
                yield return new ValidationResult("The field must be greater than 00.00", new[] { nameof(Amount) });
            }
        }
    }
}