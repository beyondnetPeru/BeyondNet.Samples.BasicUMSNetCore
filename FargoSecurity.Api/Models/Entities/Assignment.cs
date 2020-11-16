using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Assignments")]
    public class Assignment:Audit
    {
        [Key]
        public int AssignmentId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
        public bool CanRead { get; set; }
        public int Status { get; set; }
        public ICollection<AssignmentItem> AssignmentItems { get; set; }
    }
}
