using System;
using System.Collections.Generic;

namespace FargoSecurity.Api.Models.Command
{
    public class AssignmentDetailResult
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModulePath { get; set; }
        public bool ModuleCanRead { get; set; }
        public List<AssignmentDetailItemResult> Options { get; set; } = new List<AssignmentDetailItemResult>();
    }
}
