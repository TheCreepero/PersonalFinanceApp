using System.ComponentModel.DataAnnotations;

namespace Site.Models.ViewModels
{
    public class TransactionViewModel
    {
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

        public int SelectedAccount { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public DateTime TransactionDate { get; set; }

        public string TransactionId { get; set; }
    }
}