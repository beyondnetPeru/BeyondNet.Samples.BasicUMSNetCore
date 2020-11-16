using System;
using System.Linq;
using System.Threading.Tasks;
using FargoSecurity.Api.Models;
using FargoSecurity.Api.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Impl
{
    public class SecurityTokenBuilderDb : AbstractSecurityTokenBuilder
    {
        private readonly SecurityDbContext _context;

        public SecurityTokenBuilderDb(SecurityDbContext context, IConfiguration configuration)
            :base(context, configuration)
        {
            _context = context;
        }

        public async Task<string> Build(RequestLoginCommand requestLoginCommand)
        {
            try
            {
                var tokenData = await BuildTokenData(requestLoginCommand);

                var tokenSecretData = BuildTokenSecretData(tokenData.SystemCode);

                return Build(tokenData, tokenSecretData);
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al construir Token{e.Message}");
            }
        }

        protected override TokenSecretData BuildTokenSecretData(string systemCode = "")
        {
            try
            {
                var system = _context.Systems.First(e => e.Code.ToLower().Trim() == systemCode.ToLower().Trim());

                if (system == null)
                    throw new ApplicationException("Código de sistema no es válido");

                return new TokenSecretData()
                {
                    SystemCode = system.Code,
                    SecretKey = system.SecretToken,
                    ExpirationTime = system.ExpirationTimeToken == 0 ? 1 : system.ExpirationTimeToken,
                    PeriodType = string.IsNullOrEmpty(system.PeriodTypeToken) ? "d" : system.PeriodTypeToken
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al construir Token Secret{e.Message}");
            }
        }
    }
}

