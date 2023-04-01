namespace Site.Models.ViewModels
{
    public class MonthViewModel
    {
        public string AccountName { get; set; }
        public decimal StartBalance { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal EndBalance { get; set; }
    }
}