using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.System;
using FargoSecurity.Api.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SystemsController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public SystemsController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper =  new DateTimeHelper();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemInfo>>> GetSystems()
        {
            var systems= await _context.Systems.ToListAsync();

            var systemList = systems.Select(system => new SystemInfo()
                {
                    SystemId = system.SystemId,
                    Code = system.Code,
                    Name = system.Name,
                    Path = system.Path,
                    SystemType = system.SystemType,
                    Status = _auditHelper.GetStandarStatusText(system.Status)
                })
                .ToList();

            return Ok(systemList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SystemInfo>> GetSystem(int id)
        {
            var system = await _context.Systems.FindAsync(id);

            if (system == null)
                return NotFound();

            return Ok(new SystemInfo()
            {
                SystemId = system.SystemId,
                Name = system.Name,
                Code = system.Code,
                SystemType = system.SystemType,
                Path = system.Path,
                Status = _auditHelper.GetStandarStatusText(system.Status)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystem(int id, SystemEdit system)
        {
            if (!Validation(system.SystemType))
                return BadRequest();

            if (ProfileExists(id))
                return BadRequest("No es posible editar un sistema que tiene perfiles asociados.");

            var systemToUpdate = await _context.Systems.FirstOrDefaultAsync(p => p.SystemId == id);

            if (systemToUpdate == null)
                return NotFound();

            systemToUpdate.Code = system.Code;
            systemToUpdate.Name = system.Name;
            systemToUpdate.Path = system.Path;
            systemToUpdate.SystemType = system.SystemType;
            systemToUpdate.ExpirationTimeToken = system.ExpirationTimeToken;
            systemToUpdate.PeriodTypeToken = system.PeriodTypeToken;
            systemToUpdate.UpdatedDate = DateTime.Now;
            systemToUpdate.UpdatedBy = _auditHelper.MachineName();
            systemToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            systemToUpdate.Status = system.Status;

            _context.Entry(systemToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(system);
        }

        [HttpPost]
        public async Task<ActionResult<SystemAdd>> PostSystem(SystemAdd system)
        {
            if (SystemExists(system.Name))
                return BadRequest("Sistema ya existe");

            if (!Validation(system.SystemType))
                return BadRequest();

            var systemToAdd = new Models.Entities.System()
            {
                Code = system.Code,
                Name = system.Name,
                Path = system.Path,
                SystemType = system.SystemType,
                SecretToken = system.SecretToken,
                ExpirationTimeToken = system.ExpirationTimeToken,
                PeriodTypeToken = system.PeriodTypeToken,
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                TimeSpan = _dateTimeHelper.GetTimeStamp(),
                Status = 1
            };

            await _context.Systems.AddAsync(systemToAdd);

            await _context.SaveChangesAsync();

            return Ok(system);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSystem(int id)
        {
            if (ProfileExists(id))
                return BadRequest("No es posible eliminar sistema debido a que tiene perfiles asociados");

            var system = await _context.Systems.FindAsync(id);

            if (system == null)
                return NotFound();
            
            _context.Systems.Remove(system);

            await _context.SaveChangesAsync();

            return Ok("Sistema fue eliminado satisfactoriamente.");
        }

        private bool Validation(string systemType)
        {
            return systemType.ToLower().Trim() == "app" || systemType.ToLower().Trim() == "api";
        }

        private bool SystemExists(string name)
        {
            return _context.Systems.Any(e => e.Name.ToUpper().Trim() == name.Trim().ToUpper());
        }

        private bool ProfileExists(int systemId)
        {
            var profiles = from p in _context.Profiles
                join r in _context.Roles on p.Rol.RolId equals r.RolId
                where r.System.SystemId == systemId
                           select new { systemId = r.System.SystemId };

            return profiles.Any();
        }

    }
}
