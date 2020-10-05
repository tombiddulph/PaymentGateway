using System;
using System.Text.Json.Serialization;
using PaymentGateway.Models.Enums;

namespace PaymentGateway.Models.Domain
{
    public class Transaction
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        
        [JsonPropertyName("status"), JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus Status { get; set; }
    }
}