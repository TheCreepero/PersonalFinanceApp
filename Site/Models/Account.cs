using System.ComponentModel.DataAnnotations;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [Display(
        Name = "Account Name",
        Description = "Name of the bank account.",
        Order = 1)]
    public string AccountName { get; set; }

    [Editable(false)]
    [Display(
        Name = "Balance",
        Description = "Total balance of the bank account.",
        Order = 20)]
    public double AccountBalance { get; set; }
}