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
    [Route("api/systems/{systemId}/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public RolesController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolInfo>>> GetRoles(int systemId)
        {
            var roles = await _context.Roles.Where(e => e.System.SystemId == systemId).ToListAsync();

            var rolList = roles.Select(role => new RolInfo()
                {
                    RolId = role.RolId, 
                    Name = role.Name,
                    Status = _auditHelper.GetStandarStatusText(role.Status)
                });
 
            return Ok(rolList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RolInfo>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
                return NotFound();
            
            return Ok(new RolInfo(){RolId = rol.RolId, Name = rol.Name, Status = _auditHelper.GetStandarStatusText(rol.Status) });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, RolEdit rol)
        {
            var rolToUpdate = _context.Roles.First(p => p.RolId == id);

            rolToUpdate.Name = rol.Name;
            rolToUpdate.UpdatedDate = DateTime.Now;
            rolToUpdate.UpdatedBy = _auditHelper.MachineName();
            rolToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            rolToUpdate.Status = rol.Status;

            _context.Entry(rolToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(rol);
        }

        [HttpPost]
        public async Task<ActionResult<RolAdd>> PostRol(int systemId, RolAdd rol)
        {
            if (RolExists(rol.Name, systemId))
                return BadRequest("Rol ya existe para sistema seleccionado");

            var rolToAdd = new Rol()
            {
                Name = rol.Name,
                System = await _context.Systems.FirstAsync(p => p.SystemId == systemId),
                Status = 1,
                CreatedBy = _auditHelper.MachineName(),
                CreatedAt = DateTime.Now,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Roles.AddAsync(rolToAdd);

            await _context.SaveChangesAsync();

            return Ok(rol);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Rol>> DeleteRol(int id)
        {
            if (ProfileExists(id))
                return BadRequest("No se puede eliminar el rol debido a que tiene perfiles asociados");

            var rol = await _context.Roles.FindAsync(id);

            if (rol == null)
             return NotFound();
            
            _context.Roles.Remove(rol);
            
            await _context.SaveChangesAsync();

            return Ok("Rol eliminado satisfactoriamente");
        }

        private bool ProfileExists(int rolId)
        {
            return _context.Profiles.Any(e => e.Rol.RolId == rolId);
        }

        private bool RolExists(string name, int systemId)
        {
            return _context.Roles.Any(e => e.System.SystemId == systemId && e.Name == name);
        }
    }
}
