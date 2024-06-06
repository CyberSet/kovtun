using System.ComponentModel.DataAnnotations;

namespace kovtun.Models
{
    public class Operation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int WorkplaceId { get; set; }
        public virtual Workplace Workplace { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
