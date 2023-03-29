﻿using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(
            Name = "Account",
            Description = "Account the transaction was made to/from.",
            Order = 1)]
        public Account TransactionFor { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}