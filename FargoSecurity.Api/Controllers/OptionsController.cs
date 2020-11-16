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
    [Route("api/systems/{systemId}/modules/{moduleId}/[controller]")]
    [ApiController]
    [Authorize]
    public class OptionsController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public OptionsController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OptionInfo>>> GetOptions(int moduleId)
        {
            var options = await _context.Options.Where(e => e.Module.ModuleId == moduleId).ToListAsync();

            var optionList = options.Select(option => new OptionInfo()
                {
                    OptionId = option.OptionId,
                    Name = option.Name,
                    Path = option.Path,
                    Status = _auditHelper.GetStandarStatusText(option.Status)
                })
                .ToList();

            return Ok(optionList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OptionInfo>> GetOption(int id)
        {
            var option = await _context.Options.FindAsync(id);

            if (option == null)
            {
                return NotFound();
            }

            var optionInfo = new OptionInfo()
            {
                OptionId= option.OptionId,
                Name = option.Name,
                Path = option.Path,
                Status = _auditHelper.GetStandarStatusText(option.Status)
            };

            return Ok(optionInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOption(int id, OptionEdit option)
        {
            var optionToUpdate = _context.Options.First(p => p.OptionId == id);

            if (optionToUpdate == null)
                return NoContent();

            optionToUpdate.Name = option.Name;
            optionToUpdate.Path = option.Path;
            optionToUpdate.UpdatedDate = DateTime.Now;
            optionToUpdate.UpdatedBy = _auditHelper.MachineName();
            optionToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            optionToUpdate.Status = option.Status;

            _context.Entry(optionToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(option);
        }

        [HttpPost]
        public async Task<ActionResult<Option>> PostOption(int moduleId, OptionAdd option)
        {
            if (OptionExists(option.Name, moduleId))
                return BadRequest();

            var module = await _context.Modules.FirstOrDefaultAsync(e => e.ModuleId == moduleId);

            if (module == null)
                return BadRequest("Modulo no existe");
            
            var optionToAdd = new Option()
            {
                Name = option.Name,
                Path = option.Path,
                Module = module,
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                Status = 1,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Options.AddAsync(optionToAdd);

            await _context.SaveChangesAsync();

            return Ok(option);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Option>> DeleteOption(int id)
        {
           var option = await _context.Options.FindAsync(id);

            if (option == null)
                return NotFound();

            _context.Options.Remove(option);

            await _context.SaveChangesAsync();

            return option;
        }

       
        private bool OptionExists(string name, int moduleId)
        {
            return _context.Options.Any(e => e.Name == name && e.Module.ModuleId == moduleId);
        }
    }
}
