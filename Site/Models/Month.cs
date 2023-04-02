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
        public string MonthName { get; set; } // name of the month (e.g. "January")

        [Required]
        public DateTime StartDate { get; set; } // start date of the month

        [Required]
        public DateTime EndDate { get; set; } // end date of the month

        [Required]
        public decimal StartBalance { get; set; } // starting balance for the month

        [Required]
        public decimal EndBalance { get; set; } // ending balance for the month
    }
}