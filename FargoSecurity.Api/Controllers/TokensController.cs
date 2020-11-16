using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FargoSecurity.Api.Models.Entities;
using FargoSecurity.Api.Models.Profile;
using FargoSecurity.Api.Utilities;
using Microsoft.Extensions.Configuration;

namespace FargoSecurity.Api.Controllers
{
    [Route("api/profiles/{profileId}/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        private readonly IConfiguration _configuration;

        private readonly AuditHelper _auditHelper;

        private readonly DateTimeHelper _dateTimeHelper;

        public TokensController(SecurityDbContext context, IConfiguration configuration)
        {
            _context = context;

            _configuration = configuration;

            _auditHelper = new AuditHelper();

            _dateTimeHelper = new DateTimeHelper();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TokenInfo>>> GetTokens(int profileId)
        {
            var tokens = await _context.Token.Where(e => e.Profile.ProfileId == profileId).ToListAsync();

            var tokenList = tokens.Select(token => new TokenInfo()
            {
                TokenId = token.TokenId, 
                PublicToken = token.PublicToken, 
                Status = _auditHelper.GetStandarStatusText(token.Status)
            }).ToList();

            return Ok(tokenList);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TokenInfo>> GetToken(int id)
        {
            var token = await _context.Token.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            var tokenInfo = new TokenInfo()
            {
                TokenId = token.TokenId,
                PublicToken = token.PublicToken,
                Status = _auditHelper.GetStandarStatusText(token.Status)
            };

            return Ok(tokenInfo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToken(int id, TokenEdit token)
        {
            var tokenToUpdate = _context.Token.First(p => p.TokenId == id);

            tokenToUpdate.UpdatedDate = DateTime.Now;
            tokenToUpdate.UpdatedBy = _auditHelper.MachineName();
            tokenToUpdate.TimeSpan = _dateTimeHelper.GetTimeStamp();
            tokenToUpdate.Status = token.Status;

            _context.Entry(tokenToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(token);
        }
        

        [HttpDelete("{id}")]
        public async Task<ActionResult<Token>> DeleteToken(int id)
        {
            var token = await _context.Token.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Token.Remove(token);
            await _context.SaveChangesAsync();

            return token;
        }

    }
}
