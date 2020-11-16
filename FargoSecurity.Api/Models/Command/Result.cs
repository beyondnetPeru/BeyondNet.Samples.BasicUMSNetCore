using System.Collections.Generic;

namespace FargoSecurity.Api.Models.Command
{
    public class Result
    {
        public bool IsAuthenticated { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public Dictionary<string, object> Errors { get; set; }
    }
}
