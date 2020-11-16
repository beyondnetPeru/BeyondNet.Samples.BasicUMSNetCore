using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FargoSecurity.Api.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.User;
using FargoSecurity.Api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly DateTimeHelper _dateTimeHelper;

        private readonly AuditHelper _auditHelper;

        public UsersController(SecurityDbContext context, IConfiguration configuration)
        {
            _context = context;

            _configuration = configuration;

            _dateTimeHelper = new DateTimeHelper();

            _auditHelper= new AuditHelper();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            var userList = users.Select(user => new UserInfo()
                {
                    Email = user.Email,
                    Name = user.Name,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserType = user.UserType,
                    Status = _auditHelper.GetStandarStatusText(user.Status)
                })
                .ToList();

            return Ok(userList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(new UserInfo
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Name = user.Name,
                UserType = user.UserType,
                Email = user.Email,
                Status = _auditHelper.GetStandarStatusText(user.Status)
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserEdit user)
        {
            if (!Validations(user.UserType))
                return BadRequest();

            var profiles = await _context.Profiles.Where(p => p.User.UserId == id).ToListAsync();

            if (profiles.Any())
                return BadRequest("No es posible editar un usuario que tiene perfiles asociados");


            var userToUpdate = await _context.Users.FirstAsync(p => p.UserId == id);

            if (userToUpdate == null)
                return NotFound();

            userToUpdate.UserType = user.UserType;
            userToUpdate.Email = user.Email;
            userToUpdate.UserName = user.UserName;
            userToUpdate.Name = user.Name;
            if(!string.IsNullOrEmpty(user.Password)) userToUpdate.Password = Encrypt(user.Password);
            userToUpdate.Status = user.Status;
            userToUpdate.UserType = user.UserType;
            userToUpdate.UpdatedDate = DateTime.Now;
            userToUpdate.UpdatedBy = _auditHelper.MachineName();

            _context.Entry(userToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserAdd>> PostUser(UserAdd user)
        {
            if (!Validations(user.UserType))
                return BadRequest();

            if (UserExists(user.Email, user.UserName))
                return BadRequest("El usuario o correo electrónico ya se encuentran registrados");

            var userToAdd = new User()
            {
                UserName = user.UserName,
                UserType = user.UserType,
                Name = user.Name,
                Password = Encrypt(user.Password),
                Email = user.Email,
                CreatedAt = DateTime.Now,
                CreatedBy = _auditHelper.MachineName(),
                Status = 1,
                TimeSpan = _dateTimeHelper.GetTimeStamp()
            };

            await _context.Users.AddAsync(userToAdd);
            
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var profiles = await _context.Profiles.AnyAsync(p => p.User.UserId == id);

            if (profiles)
                BadRequest("Usuario no puede ser eliminado debido a que tiene perfiles asociados");

            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                return NotFound();
            
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado satisfactoriamente");
        }

        private bool UserExists(string email, string userName)
        {
            return _context.Users.Any(e => e.Email.ToUpper().Trim() == email.ToUpper().Trim() || e.UserName.ToUpper().Trim() == userName.ToUpper().Trim());
        }

        private string Encrypt(string password)
        {
            var encryption =  new TripleDesStringEncryptor(_configuration);

            return encryption.Encrypt(password);
        }

        private bool Validations(string userType)
        {
            return userType != null && (userType.ToLower().Trim() == "int" || userType.ToLower().Trim() == "ext");
        }
    }
}
