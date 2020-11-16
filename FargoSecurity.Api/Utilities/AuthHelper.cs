using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FargoSecurity.Api.Models.Command;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.System;
using FargoSecurity.Api.Models.User;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Utilities
{
    public class AuthHelper
    {
        private readonly SecurityDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly AuditHelper _auditHelper;

        public AuthHelper(SecurityDbContext context, IConfiguration configuration)
        {
            _context = context;

            _configuration = configuration;

            _auditHelper = new AuditHelper();
        }

        public bool IsInternalConfigSecret(string systemCode)
        {
            var internalSystemCode = _configuration.GetValue<string>("Setup:SystemCode");

            return !string.IsNullOrEmpty(internalSystemCode) && systemCode == internalSystemCode;
        }

        public async Task<SystemInfo> GetSystem(string systemCode)
        {
            try
            {
                var system = await _context.Systems.FirstAsync(e => e.Code.Trim().ToLower().Equals(systemCode.Trim().ToLower()));

                return new SystemInfo()
                {
                    SystemId = system.SystemId,
                    Name = system.Name,
                    Code = system.Code,
                    Status = _auditHelper.GetStandarStatusText(system.Status),
                    SystemType = system.SystemType,
                    Path = system.Path,
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de obtener sistema. {e.Message}");
            }
        }

        public int GetProfileId(string userName, string systemCode)
        {
            try
            {
                var result = from p in _context.Profiles
                    join r in _context.Roles on p.Rol.RolId equals r.RolId
                    where p.User.UserName.ToLower().Trim() == userName.ToLower().Trim() &&
                          r.System.Code.ToLower().Trim() == systemCode.ToLower().Trim()
                    select new {p.ProfileId};

                if (!result.Any())
                    throw new ApplicationException("Usuario y código de sistema no son validos");

                return result.First().ProfileId;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de obtener perfil del usuario. {e.Message}");
            }
        }
        public async Task<UserInfo> GetUser(string userName)
        {
            try
            {
                var user = await _context.Users.FirstAsync(e => e.UserName == userName);

                return new UserInfo()
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserType = user.UserType,
                    Status = _auditHelper.GetStandarStatusText(user.Status)
                };
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al tratar de obtener usuario. {e.Message}");
            }
        }

        public AssignmentResult BuildGraph(List<RawGraphData> data)
        {
            try
            {
                var system = data.First();

                var assignmentResult = new AssignmentResult
                {
                    SystemId = system.SystemId,
                    SystemName = system.SystemName,
                    SystemPath = system.SystemPath,
                    RolId = system.RolId,
                    RolName = system.RolName
                };

                var groupByModule = from p in data
                    group p by p.ModuleId
                    into modules
                    select new { moduleId = modules.Key, Data = modules.ToList() };


                foreach (var module in groupByModule)
                {
                    var mod = module.Data.First(p => p.ModuleId == module.moduleId);

                    var assignmentDetailResult = new AssignmentDetailResult
                    {
                        ModuleId = module.moduleId,
                        ModuleName = mod.ModuleName,
                        ModulePath = mod.ModulePath,
                        ModuleCanRead = mod.ModuleCanRead
                    };

                    foreach (var option in module.Data)
                    {
                        var assignmentDetailItemResult = new AssignmentDetailItemResult
                        {
                            OptionId = option.OptionId,
                            OptionName = option.OptionName,
                            OptionPath = option.OptionPath,
                            OptionCanRead = option.OptionCanRead
                        };

                        assignmentDetailResult.Options.Add(assignmentDetailItemResult);
                    }

                    assignmentResult.Modules.Add(assignmentDetailResult);
                }

                return assignmentResult;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Error al crear grafo de autorizaciones. {e.Message}");
            }
        }

    }
}
