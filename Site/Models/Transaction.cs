using System.ComponentModel.DataAnnotations;

namespace Site.Models
{
  public class Transaction
  {
      [Key]
      public string TransactionId { get; set; }

      [Required]
      [Display(
          Name = "Account Name",
          Description = "Name of the bank account.",
          Order = 1)]
      public string TransactionType{get; set;}

      [Required]
      [Display(
          Name = "Account Name",
          Description = "Name of the bank account.",
          Order = 1)]
      public decimal TransactionAmount {get; set;}

    }
}
