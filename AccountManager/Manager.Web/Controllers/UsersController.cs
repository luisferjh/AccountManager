using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Manager.Data;
using Manager.Entities;
using Manager.Web.Models.User;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace Manager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbContextManager _context;
        private readonly IConfiguration _config;

        public UsersController(DbContextManager context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // GET: api/Users/ToList
        [Authorize]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserViewModel>> ToList()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserViewModel
            {
                IdUser = u.IdUser,
                Name = u.Name,
                Email = u.Email,
                Password_hash = u.Password_hash,
                Condition = u.Condition,

            });
        }

        // GET: api/Users/Create
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Name = model.Name,
                Email = model.Email,
                //se encripta el password
                Password_hash = passwordHash, // el password encriptado se guarda aca          
                Password_salt = passwordSalt, // aca se guarda la llave con la que hemos encriptado el password
                Condition = true
            };

            _context.Users.Add(user);
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


        // PUT: api/Users
        [Authorize]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.IdUser <= 0)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(a => a.IdUser == model.IdUser);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = model.Name;
            user.Email = model.Email;

            //si act_password es igual a true se actualiza o se genera de nuevo la contraseña
            if (model.act_password == true)
            {
                CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.Password_hash = passwordHash;
                user.Password_salt = passwordSalt;
            }

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

        //PUT: api/User/Deactivate
        [Authorize]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Deactivate([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Condition = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Guardar excepcion
                return BadRequest();
            }

            return Ok();
        }

        //PUT: api/User/activate
        [Authorize]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activate([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Condition = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Guardar excepcion
                return BadRequest();
            }

            return Ok();
        }

        //[EnableCors("Todos")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var email = model.Email;

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            Console.WriteLine(user);

            if (user == null)
            {
                return NotFound();
            }

            if (!CheckPasswordHash(model.Password, user.Password_hash, user.Password_salt))
            {
                return NotFound();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("IdUser", user.IdUser.ToString()),
                new Claim("Name", user.Name)
            };
            
            return Ok(
                    new {token = GenerateToken(claims)}
                );
        }



        // con este metodo verificamos si el usuario podra ingresar o no
        private bool CheckPasswordHash(string password, byte[] passwordHashStored, byte[] passwordSaltStored)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSaltStored))
            {
                var NewPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashStored).SequenceEqual(new ReadOnlySpan<byte>(NewPasswordHash));
            }
        }

        private string GenerateToken(List<Claim> claims) 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                expires:DateTime.Now.AddMinutes(5),
                signingCredentials: creds,
                claims:claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Metodo para encriptar el password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.IdUser == id);
        }
    }
}
