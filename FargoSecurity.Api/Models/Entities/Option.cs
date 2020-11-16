using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Options")]
    public class Option:Audit
    {
        [Key]
        public int OptionId { get; set; }
        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Status { get; set; }
        public ICollection<AssignmentItem> AssignmentItems { get; set; }
    }
}
