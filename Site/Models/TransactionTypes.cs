using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Site.Models
{
    public class TransactionTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(
        Name = "Type",
        Description = "Name of the type.",
        Order = 1)]
        public string Name { get; set; }
    }
}