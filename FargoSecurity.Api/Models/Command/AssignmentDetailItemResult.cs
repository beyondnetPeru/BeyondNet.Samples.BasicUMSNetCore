using System;

namespace FargoSecurity.Api.Models.Command
{
    public class AssignmentDetailItemResult
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public string OptionPath { get; set; }
        public bool OptionCanRead { get; set; }
    }
}
