namespace Site.Models
{
    public class AccountSummary
    {
        public string AccountName { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public decimal TotalSpent { get; set; }
    }
}