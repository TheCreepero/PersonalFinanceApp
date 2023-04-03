namespace Site.Models.ViewModels
{
    public class BudgetViewModel
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int MonthId { get; set; }
        public string Amount { get; set; }
    }

    public class IndexBudgetViewModel
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string MonthName { get; set; }
        public string Amount { get; set; }
    }
}