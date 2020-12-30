using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountRegistrationAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace AccountRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountDetailsController : ControllerBase
    {
        private readonly AccountDetailContext _context;

        public AccountDetailsController(AccountDetailContext context)
        {
            _context = context;
        }

        // GET: api/AccountDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDetail>>> GetAccountDetails()
        {
            return await _context.AccountDetails.ToListAsync();
        }

        // GET: api/AccountDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetail>> GetAccountDetail(int id)
        {
            var accountDetail = await _context.AccountDetails.FindAsync(id);

            if (accountDetail == null)
            {
                return NotFound();
            }

            return accountDetail;
        }

        // PUT: api/AccountDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountDetail(int id, AccountDetail accountDetail)
        {
            if (id != accountDetail.AccountID)
            {
                return BadRequest();
            }

            _context.Entry(accountDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccountDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AccountDetail>> PostAccountDetail(AccountDetail accountDetail)
        {
            _context.AccountDetails.Add(accountDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountDetail", new { id = accountDetail.AccountID }, accountDetail);
        }

        // DELETE: api/AccountDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountDetail(int id)
        {
            var accountDetail = await _context.AccountDetails.FindAsync(id);
            if (accountDetail == null)
            {
                return NotFound();
            }

            _context.AccountDetails.Remove(accountDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountDetailExists(int id)
        {
            return _context.AccountDetails.Any(e => e.AccountID == id);
        }


        //POST: api/AccountDetails/


       
    }
}
