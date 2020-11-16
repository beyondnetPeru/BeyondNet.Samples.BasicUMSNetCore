using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FargoSecurity.Api.Utilities
{
    public class DateTimeHelper
    {
        public string GetTimeStamp()
        {
            var ticks = DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks;
            ticks /= 10000000; 
            return ticks.ToString();
        }
    }
}
