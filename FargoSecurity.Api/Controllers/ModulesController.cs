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
    public class ModulesController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public ModulesController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleInfo>>> GetModules(int systemId)
        {
            var modules = await _context.Modules.Where(e => e.System.SystemId == systemId).ToListAsync();

            var moduleList = modules.Select(module => new ModuleInfo() {ModuleId = module.ModuleId, Name = module.Name, Path = module.Path, Status = _auditHelper.GetStandarStatusText(module.Status)}).ToList();

            return Ok(moduleList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModuleInfo>> GetModule(int id)
        {
            var module = await _context.Modules.FindAsync(id);

            if (module == null)
                return NotFound();

            var moduleInfo = new ModuleInfo()
            {
                ModuleId = module.ModuleId,
                Name = module.Name,
                Path = module.Path,
                Status = _auditHelper.GetStandarStatusText(module.Status)
            };

            return Ok(moduleInfo);
        }

   
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, ModuleEdit module)
        {
            var moduleToUpdate = await _context.Modules.FirstOrDefaultAsync(p => p.ModuleId == id);

            if (moduleToUpdate == null)
                return NoContent();

            moduleToUpdate.Name = module.Name;
            moduleToUpdate.Path = module.Path;
            moduleToUpdate.UpdatedDate = DateTime.Now;
            moduleToUpdate.UpdatedBy = _auditHelper.MachineName();
            moduleToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            moduleToUpdate.Status = module.Status;

            _context.Entry(moduleToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(module);
        }

        [HttpPost]
        public async Task<ActionResult<ModuleAdd>> PostModule(ModuleAdd module)
        {
            if (ModuleExists(module.Name, module.SystemId))
                return BadRequest();

            var moduleToAdd = new Module()
            {
                Name = module.Name,
                Path = module.Path,
                System = await _context.Systems.FirstAsync(p => p.SystemId == module.SystemId),
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                Status = 1,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Modules.AddAsync(moduleToAdd);

            await _context.SaveChangesAsync();

            return Ok(module);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Module>> DeleteModule(int id)
        {
            if (OptionsExists(id))
                return BadRequest("No es posible eliminar el module debido a que tiene opciones asignadas");

            if (AssignmentsExists(id))
                return BadRequest("No es posible eliminar el module debido a que tiene asignaciones");

            var module = await _context.Modules.FindAsync(id);
            
            if (module == null)
                return NotFound();
            

            _context.Modules.Remove(module);
            await _context.SaveChangesAsync();

            return Ok("Modulo eliminado satisfactoriamente");
        }

        private bool ModuleExists(string name, int systemId)
        {
            return _context.Modules.Any(e => e.System.SystemId == systemId && e.Name == name);
        }

        private bool OptionsExists(int moduleId)
        {
            return _context.Options.Any(e => e.Module.ModuleId == moduleId);
        }

        private bool AssignmentsExists(int moduleId)
        {
            return _context.Assignments.Any(e => e.Module.ModuleId == moduleId);
        }
    }
}
