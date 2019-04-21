using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Manager.Data;
using Manager.Entities;
using Manager.Web.Models.Account;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Manager.Web.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DbContextManager _context;

        public AccountsController(DbContextManager context)
        {
            _context = context;
        }


        // GET: api/Accounts
        //[EnableCors("Todos")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<AccountViewModel>> ToList()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts.Select(a => new AccountViewModel
            {
                IdAccount = a.IdAccount,
                WebAccountName = a.WebAccountName,
                UserAccount = a.UserAccount,
                Password = a.Password,
                Description = a.Description,
                Email = a.Email
            });
        }

        // GET: api/Accounts/ShowBasicData

        [HttpGet("[action]")]
        public async Task<IEnumerable<AccountBasicDataViewModel>> MainData()
        {
            var accounts = await _context.Accounts.ToListAsync();

            return accounts.Select(a => new AccountBasicDataViewModel
            {
                IdAccount = a.IdAccount,
                WebAccountName = a.WebAccountName,
                Email = a.Email
            });
        }

        // GET: api/Accounts/5
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Show([FromRoute] int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(new AccountViewModel
            {
                IdAccount = account.IdAccount,
                WebAccountName = account.WebAccountName,
                UserAccount = account.UserAccount,
                Password = account.Password,
                Description = account.Description,
                Email = account.Email
            });
        }

        // GET: api/Accounts/search/searchString
        [HttpGet("[action]/{searchString}")]
        public async Task<IEnumerable<AccountViewModel>> Search([FromRoute] string searchString)
        {
            var accounts = await _context.Accounts
                .Where(a => a.WebAccountName.Contains(searchString))
                .ToListAsync();

            return accounts.Select(a => new AccountViewModel
            {
                IdAccount = a.IdAccount,
                WebAccountName = a.WebAccountName,
                UserAccount = a.UserAccount,
                Password = a.Password,
                Description = a.Description,
                Email = a.Email
            });

        }

        // PUT: api/Accounts/
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdAccount <= 0)
            {
                return BadRequest();
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.IdAccount == model.IdAccount);

            if (account == null)
            {
                return NotFound();
            }

            account.WebAccountName = model.WebAccountName;
            account.UserAccount = model.UserAccount;
            account.Password = model.Password;
            account.Description = model.Description;
            account.Email = model.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Accounts
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Account account = new Account
            {
                WebAccountName = model.WebAccountName,
                UserAccount = model.UserAccount,
                Password = model.Password,
                Description = model.Description,
                Email = model.Email
            };

            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();

        }

        // DELETE: api/Accounts/5
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(account);
        }
        //public async Task<ActionResult<Account>> DeleteAccount(int id)
        //{
        //    var account = await _context.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Accounts.Remove(account);
        //    await _context.SaveChangesAsync();

        //    return account;
        //}

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.IdAccount == id);
        }
    }
}
