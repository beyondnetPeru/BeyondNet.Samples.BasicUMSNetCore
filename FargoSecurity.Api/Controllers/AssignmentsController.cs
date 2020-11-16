using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.Profile;
using FargoSecurity.Api.Utilities;
using System;
using Microsoft.AspNetCore.Authorization;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/profiles/{profileId}/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public AssignmentsController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentInfo>>> GetAssignments(int profileId)
        {
            var assignments = await _context.Assignments.Where(p => p.Profile.ProfileId == profileId).ToListAsync();
            
            var assignmentList = assignments.Select(assignment => new AssignmentInfo()
                {
                    AssignmentId = assignment.AssignmentId,
                    ProfileId = assignment.Profile.ProfileId,
                    ModuleId = assignment.Module.ModuleId,
                    CanRead = assignment.CanRead,
                    Status = _auditHelper.GetStandarStatusText(assignment.Status)
                })
                .ToList();

            return Ok(assignmentList);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentInfo>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment == null)
                return NotFound();

            var assignmentInfo = new AssignmentInfo()
            {
                AssignmentId = assignment.AssignmentId,
                ProfileId = assignment.Profile.ProfileId,
                ModuleId = assignment.Module.ModuleId,
                ModuleName = assignment.Module.Name,
                CanRead = assignment.CanRead,
                Status = _auditHelper.GetStandarStatusText(assignment.Status)
            };

            return assignmentInfo;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, AssignmentEdit assignment)
        {
           var assignmentToUpdate = _context.Assignments.First(p => p.AssignmentId == id);

            assignmentToUpdate.CanRead = assignment.CanRead;
            assignmentToUpdate.UpdatedDate = DateTime.Now;
            assignmentToUpdate.UpdatedBy = _auditHelper.MachineName();
            assignmentToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            assignmentToUpdate.Status = assignment.Status;

            _context.Entry(assignmentToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(assignment);
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentAdd>> PostAssignment(int profileId, AssignmentAdd assignment)
        {
            var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.ProfileId == profileId);

            var module = await _context.Modules.FirstOrDefaultAsync(p => p.ModuleId == assignment.ModuleId);

            if (profile == null || module == null)
                return NoContent();

            var assignmentToAdd = new Assignment()
            {
                CanRead = assignment.CanRead,
                Profile = profile,
                Module = module,
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                Status = 1,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Assignments.AddAsync(assignmentToAdd);

            await _context.SaveChangesAsync();

            return Ok(assignment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Assignment>> DeleteAssignment(int id)
        {
            if (AssignmentItemsExists(id))
                return BadRequest("No se puede eliminar assignación de permisos debido a que tiene permisos asociados");

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return assignment;
        }

        private bool AssignmentItemsExists(int assignmentId)
        {
            return _context.AssignmentItems.Any(e => e.Assignment.AssignmentId == assignmentId);
        }

        private bool AssignmentExists(int assignmentId, int moduleId)
        {
            return _context.Assignments.Any(e => e.AssignmentId == assignmentId && e.Module.ModuleId == moduleId && e.CanRead && e.Status ==1);
        }
    }
}
