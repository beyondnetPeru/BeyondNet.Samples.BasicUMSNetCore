using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FargoSecurity.Api.Models.Entities
{
    [Table(("Systems"))]
    public class System:Audit
    {
        [Key]
        public int SystemId { get; set; }
        public string SystemType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string SecretToken { get; set; }
        public string PeriodTypeToken { get; set; }
        public int ExpirationTimeToken { get; set; }
        public int Status { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Rol> Roles { get; set; }
    }
}
