namespace Site.Models.ViewModels
{
    public class SummaryTableRowViewModel
    {
        public string AccountName { get; set; }
        public decimal BeginningBalance { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal EndingBalance { get; set; }
    }
}