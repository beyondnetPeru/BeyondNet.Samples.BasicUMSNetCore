using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Roles")]
    public class Rol:Audit
    {   
        [Key]
        public int RolId { get; set; }
        [ForeignKey("SystemId")]
        public System System { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public ICollection<Profile> Profiles { get; set; }
    }
}
