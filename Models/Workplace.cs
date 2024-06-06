using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace kovtun.Models
{
    public class Workplace
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
