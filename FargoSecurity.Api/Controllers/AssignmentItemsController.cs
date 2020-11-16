using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.Profile;
using FargoSecurity.Api.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/profiles/{profileId}/assignments/{assignmentId}/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentItemsController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public AssignmentItemsController(SecurityDbContext context)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentItemInfo>>> GetAssignmentItems(int assignmentId)
        {
            var assignmentItems = await _context.AssignmentItems.ToListAsync();

            var assignmentItemList = assignmentItems.Select(assignmentItem => new AssignmentItemInfo()
                {
                    AssignmentItemId = assignmentItem.AssignmentItemId,
                    AssignmentId = assignmentId,
                    OptionId = assignmentItem.Option.OptionId,
                    OptionName = assignmentItem.Option.Name,
                    CanRead = assignmentItem.CanRead,
                    Status = _auditHelper.GetStandarStatusText(assignmentItem.Status)
                })
                .ToList();

            return Ok(assignmentItemList);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentItem>> GetAssignmentItem(int id)
        {
            var assignmentItem = await _context.AssignmentItems.FindAsync(id);

            if (assignmentItem == null)
            {
                return NotFound();
            }

            var assignmentInfo = new AssignmentItemInfo()
            {
                AssignmentItemId = assignmentItem.AssignmentItemId,
                AssignmentId = assignmentItem.Assignment.AssignmentId,
                OptionId = assignmentItem.Option.OptionId,
                OptionName = assignmentItem.Option.Name,
                CanRead = assignmentItem.CanRead,
                Status = _auditHelper.GetStandarStatusText(assignmentItem.Status)
            };
            
            return Ok(assignmentInfo);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignmentItem(int id, AssignmentItemEdit assignmentItem)
        {
            if (AssignmentItemExists(assignmentItem.AssignmentId, id))
                return BadRequest("Detalle de asignación ya existe para asignación seleccionada");

            var assignmentItemToUpdate = _context.AssignmentItems.First(p => p.AssignmentItemId == id);

            assignmentItemToUpdate.CanRead = assignmentItem.CanRead;
            assignmentItemToUpdate.UpdatedDate = DateTime.Now;
            assignmentItemToUpdate.UpdatedBy = _auditHelper.MachineName();
            assignmentItemToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            assignmentItemToUpdate.Status = assignmentItem.Status;

            _context.Entry(assignmentItemToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(assignmentItemToUpdate);
        }

        [HttpPost]
        public async Task<ActionResult<AssignmentItemAdd>> PostAssignmentItem(AssignmentItemAdd assignmentItem)
        {
            var assignmentToAdd = new AssignmentItem()
            {
                CanRead = assignmentItem.CanRead,
                Option = await _context.Options.FirstAsync(p => p.OptionId == assignmentItem.OptionId),
                Assignment = await _context.Assignments.FirstAsync(p => p.AssignmentId == assignmentItem.AssignmentId),
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                Status = 1,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.AssignmentItems.AddAsync(assignmentToAdd);

            await _context.SaveChangesAsync();

            return Ok(assignmentItem);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AssignmentItem>> DeleteAssignmentItem(int id)
        {
            var assignmentItem = await _context.AssignmentItems.FindAsync(id);
            if (assignmentItem == null)
            {
                return NotFound();
            }

            _context.AssignmentItems.Remove(assignmentItem);
            await _context.SaveChangesAsync();

            return assignmentItem;
        }

        private bool AssignmentItemExists(int assignmentId, int assignmentItemId)
        {
            return _context.AssignmentItems.Any(e => e.AssignmentItemId == assignmentItemId && e.Assignment.AssignmentId == assignmentId);
        }
    }
}
