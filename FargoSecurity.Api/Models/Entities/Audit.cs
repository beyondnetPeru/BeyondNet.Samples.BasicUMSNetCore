using System;

namespace FargoSecurity.Api.Models.Entities
{
    public class Audit
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TimeSpan { get; set; }

        public Audit()
        {
            UpdatedBy = null;
            UpdatedDate = null;
        }
    }
}
