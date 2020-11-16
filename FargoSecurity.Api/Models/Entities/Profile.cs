using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Profiles")]
    public class Profile:Audit
    {
        [Key]
        public int ProfileId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("RoleId")]
        public Rol Rol { get; set; }
        public string NickName { get; set; }
        public int Status { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}
