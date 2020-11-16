using System;
using System.Collections.Generic;

namespace FargoSecurity.Api.Models.Command
{
    public class AssignmentResult
    {
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemPath { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; }
        public List<AssignmentDetailResult> Modules { get; set; } =  new List<AssignmentDetailResult>();
    }
}
