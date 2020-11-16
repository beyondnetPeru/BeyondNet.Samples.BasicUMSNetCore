using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FargoSecurity.Api.Impl;
using FargoSecurity.Api.Models;
using FargoSecurity.Api.Models.Command;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly AuthHelper _authHelper;

        public AuthController(SecurityDbContext context, IConfiguration configuration)
        {
            _context = context;

            _authHelper =  new AuthHelper(context, configuration);
            
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(RequestLoginCommand requestLoginCommand)
        {  
            if (!LoginValid(requestLoginCommand)) return BadRequest();

            var encryption = new TripleDesStringEncryptor(_configuration);

            var encryptedPassword = encryption.Encrypt(requestLoginCommand.Password);

            var user = _context.Users.FirstOrDefault(p =>
                p.UserName.ToLower().Trim() == requestLoginCommand.UserName.ToLower().Trim());

            if (user == null || user.Password != encryptedPassword)
                return BadRequest("Usuario y/o contraseña no válida");

            return Ok(await BuildResult(requestLoginCommand));

        }

        [HttpGet("{profileId}")]
        [Authorize]
        public IActionResult GetAuthorization(int profileId)
        {

            var result = from o in _context.Options
                join m in _context.Modules on o.Module.ModuleId equals m.ModuleId
                join ai in _context.AssignmentItems on o.OptionId equals ai.Option.OptionId
                join a in _context.Assignments on m.ModuleId equals a.Module.ModuleId
                join p in _context.Profiles on a.Profile.ProfileId equals p.ProfileId
                join u in _context.Users on p.User.UserId equals u.UserId
                join r in _context.Roles on p.Rol.RolId equals r.RolId
                join s in _context.Systems on r.System.SystemId equals s.SystemId
                         where p.ProfileId == profileId && p.Status == 1
                select new RawGraphData()
                {
                    SystemId = s.SystemId,
                    SystemName = s.Name,
                    SystemPath = s.Path,
                    ModuleId = m.ModuleId,
                    ModuleName = m.Name,
                    ModulePath = m.Path,
                    ModuleCanRead = a.CanRead,
                    OptionId = o.OptionId,
                    OptionName = o.Name,
                    OptionPath = o.Path,
                    OptionCanRead = ai.CanRead,
                    RolId = r.RolId,
                    RolName = r.Name
                };

            if (!result.Any())
                return BadRequest("Usuario no tiene asociado un perfil para el sistema");

            var data = new Dictionary<string, object> {{"Graph", _authHelper.BuildGraph(result.ToList())}};

            return Ok(new Result(){ IsAuthenticated= true, Data = data} );
        }


        #region Internal Methods

        private async Task<Result> BuildResult(RequestLoginCommand requestLoginCommand)
        {
            var data = new Dictionary<string, object>
            {
                {"ProfileId", _authHelper.GetProfileId(requestLoginCommand.UserName, requestLoginCommand.SystemCode)},
                {"Token", await BuildToken(requestLoginCommand)}
            };

            return new Result()
            {
                IsAuthenticated = true,
                Data = data,
            };
        }

        private bool LoginValid(RequestLoginCommand requestLoginCommand)
        {
            return requestLoginCommand != null && !string.IsNullOrEmpty(requestLoginCommand.UserName) && !string.IsNullOrEmpty(requestLoginCommand.Password) && !string.IsNullOrEmpty(requestLoginCommand.SystemCode);
        }

        private async Task<string> BuildToken(RequestLoginCommand requestLoginCommand)
        {
            if (_authHelper.IsInternalConfigSecret(requestLoginCommand.SystemCode))
            {
                var tokenBuilderFromConfig = new SecurityTokenBuilderConfig(_configuration, _context);

                return await tokenBuilderFromConfig.Build(requestLoginCommand);
            }

            var tokenBuilderFromDb = new SecurityTokenBuilderDb(_context, _configuration);

            return await tokenBuilderFromDb.Build(requestLoginCommand);
        }

        #endregion

    }
}
