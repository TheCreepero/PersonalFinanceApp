using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TransactionId { get; set; }

        [Required]
        [Display(
            Name = "Name",
            Description = "Name of the transaction.",
            Order = 1)]
        public string TransactionType { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(
            Name = "Amount",
            Description = "Amount of the transaction.",
            Order = 1)]
        [Precision(18, 2)]
        public decimal TransactionAmount { get; set; }

        [Required]
        [Display(
            Name = "Account",
            Description = "Account the transaction was made to/from.",
            Order = 1)]
        public int AccountId { get; set; }

        [Display(
            Name = "Type",
            Description = "Type of the transaction.",
            Order = 1)]
        public string Type { get; set; }

        public DateTime Date { get; set; }
    }
}