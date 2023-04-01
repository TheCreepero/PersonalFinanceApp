namespace Site.Models.ViewModels
{
    public class IndexMonthViewModel
    {
        public int MonthId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal StartBalance { get; set; }
        public decimal EndBalance { get; set; }
        public IEnumerable<MonthViewModel> SummaryTable { get; set; }
    }
}