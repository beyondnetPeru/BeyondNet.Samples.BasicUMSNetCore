using System;
using System.Threading.Tasks;
using FargoSecurity.Api.Models;
using FargoSecurity.Api.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Impl
{
    public class SecurityTokenBuilderConfig : AbstractSecurityTokenBuilder
    {
        private readonly IConfiguration _configuration;

        public SecurityTokenBuilderConfig(IConfiguration configuration, SecurityDbContext context)
            :base(context, configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> Build(RequestLoginCommand requestLoginCommand)
        {
            try
            {
                var tokenData = await BuildTokenData(requestLoginCommand);

                var tokenSecretData = BuildTokenSecretData();

                return base.Build(tokenData, tokenSecretData);
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
                return new TokenSecretData()
                {
                    SystemCode = _configuration.GetValue<string>("Setup:SystemCode"),
                    SecretKey = _configuration.GetValue<string>("Jwt:SecretKey"),
                    ExpirationTime = _configuration.GetValue<int>("Jwt:ExpirationTime"),
                    PeriodType = _configuration.GetValue<string>("Jwt:PeriodType")
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al construir Token Secret{e.Message}");
            }
        }
    }
}
