using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table(("Tokens"))]
    public class Token:Audit
    {
        [Key]
        public int TokenId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }
        public string PublicToken { get; set; }
        public int Status { get; set; }
    }
}
