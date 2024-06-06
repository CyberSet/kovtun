using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace kovtun.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
