using System;

namespace FargoSecurity.Api.Models.Command
{
    public class RawGraphData
    {
        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemPath { get; set; }
        public int RolId { get; set; }
        public string RolName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModulePath { get; set; }
        public bool ModuleCanRead { get; set; }
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public string OptionPath { get; set; }
        public bool OptionCanRead { get; set; }

    }
}
