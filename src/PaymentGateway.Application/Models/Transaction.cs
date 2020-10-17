using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Application.Models
{
    public class Transaction : IModelId
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid MerchantId { get; set; }
        public virtual Merchant Merchant { get; set; }
        public Guid CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}