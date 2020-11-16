using System;
using System.Collections.Generic;
using System.Text;

namespace FargoSecurity.Api.Models.System
{
    public class SystemInfo
    {
        public int SystemId { get; set; }
        public string SystemType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Status { get; set; }
    }
}
