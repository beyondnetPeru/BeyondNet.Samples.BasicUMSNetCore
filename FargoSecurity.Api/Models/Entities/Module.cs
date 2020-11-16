using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Modules")]
    public class Module:Audit
    {
        [Key]
        public int ModuleId { get; set; }
        [ForeignKey("SystemId")]
        public System System { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Status { get; set; }
        public ICollection<Option> Options { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
    }
}
