using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FargoSecurity.Api.Utilities
{
    public class AuditHelper
    {
        public string MachineName()
        {
            return "TEMP";
        }

        public string GetStandarStatusText(int status)
        {
            return status == 1 ? "Activo" : "Inactivo";
        }
    }
}
