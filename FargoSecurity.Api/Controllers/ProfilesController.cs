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
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        private readonly IConfiguration _configuration;

        public ProfilesController(SecurityDbContext context, IConfiguration configuration)
        {
            _context = context;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();

            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileInfo>>> GetProfilesByUser(int userId)
        {
            var profiles = await _context.Profiles.Where(e => e.User.UserId == userId).ToListAsync();

            if (!profiles.Any())
                return BadRequest("Usuario no tiene perfiles asociados");

            var users = await _context.Users.FirstAsync(e => e.UserId == userId);

            var profileList = profiles.Select(profile => new ProfileInfo()
                {
                    ProfileId = profile.ProfileId,
                    NickName = profile.NickName,
                    Status = _auditHelper.GetStandarStatusText(profile.Status),
                    UserName = users.UserName
                })
                .ToList();

            return Ok(profileList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileInfo>> GetProfile(int userId, int id)
        {
            var profile = await _context.Profiles.FindAsync(id);

            var users = await _context.Users.FirstAsync(e => e.UserId == userId);

            if (profile == null)
            {
                return NotFound();
            }

            var profileInfo = new ProfileInfo()
            {
                ProfileId = profile.ProfileId,
                NickName = profile.NickName,
                Status = _auditHelper.GetStandarStatusText(profile.Status),
                UserName = users.UserName
            };

            return Ok(profileInfo);
        }

  
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, ProfileEdit profile)
        {
            if (AssignmentsExists(id))
               return BadRequest("No se puede editar un perfil que tiene asignaciones");

            var profileToUpdate = _context.Profiles.First(p => p.ProfileId == id);

            profileToUpdate.NickName = profile.NickName;
            profileToUpdate.UpdatedDate = DateTime.Now;
            profileToUpdate.UpdatedBy = _auditHelper.MachineName();
            profileToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            profileToUpdate.Status = profile.Status;

            _context.Entry(profileToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<ProfileAdd>> PostProfile(int userId, ProfileAdd profile)
        {
            if (ProfileExists(profile.RolId, profile.NickName))
                return BadRequest("Profile ya existe para rol seleccionado");

            var profileToAdd = new Profile()
            {
                NickName = profile.NickName,
                Rol = await _context.Roles.FirstAsync(p => p.RolId == profile.RolId),
                User = await _context.Users.FirstAsync(p => p.UserId == userId),
                Status = 1,
                CreatedBy = _auditHelper.MachineName(),
                CreatedAt = DateTime.Now,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Profiles.AddAsync(profileToAdd);

            await _context.SaveChangesAsync();

            return Ok(profile);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Profile>> DeleteProfile(int id)
        {
            if (AssignmentsExists(id))
                return BadRequest("No se puede eliminar un perfile porque tiene asignaciones");

            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);

            await _context.SaveChangesAsync();

            return Ok("Profile eliminado satisfactoriamente");
        }

        private bool AssignmentsExists(int profileId)
        {
            return _context.Assignments.Any(e => e.Profile.ProfileId == profileId);
        }

        private bool ProfileExists(int rolId, string nickName)
        {
            return _context.Profiles.Any(e => e.Rol.RolId == rolId && e.NickName == nickName);
        }
    }
}
