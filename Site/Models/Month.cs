using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Models
{
    public class Month
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MonthId { get; set; } // unique identifier for the month

        [Required]
        [Display(
            Name = "Name",
            Description = "Custom Name for the Month.",
            Order = 1)]
        public string MonthName { get; set; } // name of the month (e.g. "January")

        [Required]
        [Display(
            Name = "Starting Date",
            Description = "Starting date.",
            Order = 1)]
        public DateTime StartDate { get; set; } // start date of the month

        [Required]
        [Display(
            Name = "Ending Date",
            Description = "Starting date.",
            Order = 1)]
        public DateTime EndDate { get; set; } // end date of the month

        [Required]
        public decimal StartBalance { get; set; } // starting balance for the month

        [Required]
        public decimal EndBalance { get; set; } // ending balance for the month
    }
}