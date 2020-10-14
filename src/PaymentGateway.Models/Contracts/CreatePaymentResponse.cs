using System;
using System.Text.Json.Serialization;
using PaymentGateway.Models.Enums;

namespace PaymentGateway.Models.Contracts
{
    public class CreatePaymentResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("status")]
        public PaymentStatus Status { get; set; }
    }
}