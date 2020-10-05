using System.Text.Json.Serialization;

namespace PaymentGateway.Models.Domain
{
    public class Card
    {
        [JsonPropertyName("cvv")]
        public string Cvv { get; set; }
        
        [JsonPropertyName("expiryMonth")]
        public string ExpiryMonth { get; set; }

        [JsonPropertyName("expiryYear")]
        public string ExpiryYear { get; set; }

        [JsonPropertyName("holderName")]
        public string HolderName { get; set; }
        
        [JsonPropertyName("number")]
        public string Number { get; set; }
    }
}