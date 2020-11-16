using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace FargoSecurity.Api.Models.Entities
{
    [Table("Users")]
    public class User:Audit
    {
        [Key]
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public ICollection<Profile> Profiles { get; set; }
    }
}
