using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FargoSecurity.Api.Models;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Utilities;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FargoSecurity.Api.Impl
{
    public abstract class AbstractSecurityTokenBuilder
    {
        private readonly SecurityDbContext _context;

        private readonly AuthHelper _authHelper;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        protected abstract TokenSecretData BuildTokenSecretData(string systemCode = "");

        protected AbstractSecurityTokenBuilder(SecurityDbContext context, IConfiguration configuration)
        {
            _authHelper =  new AuthHelper(context, configuration);

            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper =  new DateTimeHelper();
        }

        public string Build(TokenData tokenData, TokenSecretData tokenSecretData)
        {
            try
            {
                var tokenDescriptor = BuildSecurityTokenDescriptor(tokenData, tokenSecretData);

                var tokenHandler = new JwtSecurityTokenHandler();

                var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                var token = tokenHandler.WriteToken(createdToken);

                if (!_authHelper.IsInternalConfigSecret(tokenData.SystemCode))
                    AddTokenProfile(tokenData, token);

                return token;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de crear token.{e.Message}");
            }
        }

        protected async Task<TokenData> BuildTokenData(RequestLoginCommand requestLoginCommand)
        {
            try
            {
                var system = await _authHelper.GetSystem(requestLoginCommand.SystemCode);

                var user = await _authHelper.GetUser(requestLoginCommand.UserName);

                var result = from p in _context.Profiles
                    join u in _context.Users on p.User.UserId equals u.UserId
                    join r in _context.Roles on p.Rol.RolId equals r.RolId
                    where r.System.SystemId == system.SystemId && u.UserId == user.UserId
                                                               && p.Status == 1
                    select new TokenData()
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        Email = u.Email,
                        SystemCode = r.System.Code,
                        SystemName = r.System.Name,
                        ProfileId = p.ProfileId
                    };

                if (!result.Any())
                    throw new ApplicationException("Usuario no tiene asignado un perfil para el sistema.");

                return result.First();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de construir Token Data.{e.Message}");
            }
        }

        protected async void AddTokenProfile(TokenData tokenData, string token)
        {
            try
            {
                var tokens = _context.Token.Where(e => e.Profile.ProfileId == tokenData.ProfileId && e.Status == 1);

                if (tokens.Any()) return;

                await _context.Token.AddAsync(new Token()
                {
                    Profile = await _context.Profiles.FirstAsync(e => e.ProfileId == tokenData.ProfileId),
                    PublicToken = token,
                    CreatedBy = _auditHelper.MachineName(),
                    CreatedAt = DateTime.Now,
                    TimeSpan = _dateTimeHelper.GetTimeStamp(),
                    Status = 1
                });

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de verificar token en BD.{e.Message}");
            }
           
        }

        protected SecurityTokenDescriptor BuildSecurityTokenDescriptor(TokenData tokenData, TokenSecretData tokenSecretData)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(tokenSecretData.SecretKey);

                return new SecurityTokenDescriptor
                {

                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.System, tokenData.SystemCode),
                        new Claim(ClaimTypes.UserData, tokenData.ProfileId.ToString()),
                    }),

                    
                    Expires = GetExpirationTime(tokenSecretData),

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException("Error al tratar de construir el token descriptor. " + e.Message);
            }
        }

        private DateTime GetExpirationTime(TokenSecretData tokenSecretData)
        {
            DateTime expirationTime;

            switch (tokenSecretData.PeriodType)
            {
                case "d":
                    expirationTime = DateTime.UtcNow.AddDays(tokenSecretData.ExpirationTime);
                    break;
                case "m":
                    expirationTime =  DateTime.UtcNow.AddMonths(tokenSecretData.ExpirationTime);
                    break;
                case "y":
                    expirationTime = DateTime.UtcNow.AddYears(tokenSecretData.ExpirationTime);
                    break;
                default:
                    expirationTime = DateTime.UtcNow.AddDays(1);
                    break;
            }

            return expirationTime;
        }

    }
}
