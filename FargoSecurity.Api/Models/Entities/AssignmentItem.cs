using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("AssignmentItems")]
    public class AssignmentItem:Audit
    {
        [Key]
        public int AssignmentItemId { get; set; }
        [ForeignKey("AssignmentId")]
        public Assignment Assignment { get; set; }
        [ForeignKey("OptionId")]
        public Option Option { get; set; }
        public bool CanRead { get; set; }
        public int Status { get; set; }
    }
}
