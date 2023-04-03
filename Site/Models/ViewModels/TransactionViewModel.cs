using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Site.Models.ViewModels
{
    public class TransactionViewModel
    {
        [Required]
        [Display(
            Name = "Transaction Name",
            Description = "Name of the transaction.",
            Order = 1)]
        public string TransactionName { get; set; }

        [Required]
        [Display(
            Name = "Amount",
            Description = "Amount of the transaction.",
            Order = 2)]
        public string TransactionAmount { get; set; }

        public string TransactionId { get; set; }

        public int SelectedAccount { get; set; }

        [Display(
            Name = "Category",
            Description = "Type of the transaction.",
            Order = 1)]
        public int SelectedType { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public DateTime TransactionDate { get; set; }
    }

    public class CreateTransactionViewModel
    {
        [Required]
        [Display(
            Name = "Name",
            Description = "Name of the transaction.",
            Order = 1)]
        public string TransactionName { get; set; }

        [Required]
        [Display(
            Name = "Amount",
            Description = "Amount of the transaction.",
            Order = 2)]
        public string TransactionAmount { get; set; }

        [Display(
            Name = "Account",
            Description = "Account the transaction is made to/from.",
            Order = 1)]
        public int SelectedAccount { get; set; }

        [Display(
            Name = "Category",
            Description = "Type of the transaction.",
            Order = 1)]
        public int SelectedType { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();

        public List<TransactionTypes> TransactionTypes { get; set; } = new List<TransactionTypes>();
    }

    public class IndexTransactionViewModel
    {
        [Required]
        [Display(
            Name = "Name",
            Description = "Name of the transaction.",
            Order = 1)]
        public string TransactionName { get; set; }

        [Required]
        [Display(
            Name = "Amount",
            Description = "Amount of the transaction.",
            Order = 2)]
        public decimal TransactionAmount { get; set; }

        public string TransactionId { get; set; }

        [Required]
        [Display(
            Name = "Date",
            Description = "Date the transaction was made.",
            Order = 2)]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Display(
            Name = "Account",
            Description = "Account the transaction was made to/from.",
            Order = 2)]
        public string AccountName { get; set; }

        [Display(
            Name = "Type",
            Description = "Type of the transaction.",
            Order = 1)]
        public string Type { get; set; }
    }
}