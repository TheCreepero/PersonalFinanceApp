namespace Site.Models.ViewModels
{
    public class MonthTransactionViewModel
    {
        public DateTime Month { get; set; }
        public List<Transaction> Transactions { get; set; }
        public List<AccountSummary> SummaryTable { get; set; }
        public Dictionary<int, decimal> AccountBalances { get; set; }
        public Dictionary<int, string> AccountNames { get; set; }
    }
}