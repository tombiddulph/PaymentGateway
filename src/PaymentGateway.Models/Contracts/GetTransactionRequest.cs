using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Models.Contracts
{
    public class GetTransactionRequest
    {
        [Required, FromRoute(Name = "id")]
        public Guid? Id { get; set; }
    }
}