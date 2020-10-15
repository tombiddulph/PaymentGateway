using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentGateway.Models.Contracts
{
    public class AuthenticateRequest
    {
        [Required, JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required, JsonPropertyName("password")]
        public string Password { get; set; }
    }
}