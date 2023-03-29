using System.ComponentModel.DataAnnotations;

namespace Site.Models
{
    public class Transaction
    {
        [Key]
        public string TransactionId { get; set; }

        [Required]
        [Display(
            Name = "Transaction Type",
            Description = "Type of the transaction.",
            Order = 1)]
        public string TransactionType { get; set; }

        [Required]
        [Display(
            Name = "Amount",
            Description = "Amount of the transaction.",
            Order = 1)]
        public decimal TransactionAmount { get; set; }
    }
}