using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentGateway.Data.Models
{
    [Table("cards")]
    public class Card
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Key { get; set; }
        [StringLength(3)]
        public string Cvv { get; set; }
        [StringLength(2)]
        public string ExpiryMonth { get; set; }
        [StringLength(2)]
        public string ExpiryYear { get; set; }
        [StringLength(100)]
        public string HolderName { get; set; }
        [StringLength(16)]
        public string Number { get; set; }
    }
}