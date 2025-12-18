using LinkAPI.Dto.User;
using LinkAPI.Interfaces;
using LinkAPI.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace LinkAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IUnitOfWork unitOfWork, IConfiguration configuration) : ControllerBase
    {
        [HttpPost("register")]
        [EnableCors("default")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if(unitOfWork.UserRepository.IsUserExists(userDto.Username))
            {
                var errorResponse = new { success = false, message = "User already exists" };
                return BadRequest(errorResponse);
            }
            CreatePasswordHash(userDto.Password, out var passwordHash, out var passwordSalt);
            
            var currentUser = new User()
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserRole = userDto.UserRole
            };

            unitOfWork.Save();

            return Ok(currentUser); 
        }

        [HttpPost("login")]
        [EnableCors("default")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!unitOfWork.UserRepository.IsUserExists(loginDto.Username))
            {
                var errorResponse = new { success = false, message = "Invalid password or username" };
                
                return BadRequest(errorResponse);
            }
            
            var currentUser = unitOfWork.UserRepository.GetUserByUsername(loginDto.Username);

            if(!VerifyPassword(loginDto.Password, currentUser.PasswordHash, currentUser.PasswordSalt))
            {
                var errorResponse = new { success = false, message = "Invalid password or username" };
                
                return BadRequest(errorResponse);
            }
            
            var token = CreateToken(currentUser);
            
            return Ok(new UserTokenDto() { Token = token });
        }

        private string CreateToken(User user)
        {
            var listClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.UserRole),
            };
            
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.
                GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: listClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);
            
            var jwt  = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256();
            
            passwordSalt = hmac.Key;
            
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(string password, byte[]passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA256(passwordSalt);
            
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            
            return computeHash.SequenceEqual(passwordHash);
        }
    }
}
