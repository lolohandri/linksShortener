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
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [EnableCors("default")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            if(_unitOfWork.UserRepository.IsUserExists(userDto.Username))
            {
                var errorResponse = new { message = "User already exists" };
                return BadRequest(errorResponse);
            }
            var currentUser = new User();
            CreatePasswordHash(userDto.Password, out var passwordHash, out var passwordSalt);
            currentUser.Username = userDto.Username;
            currentUser.PasswordHash = passwordHash;
            currentUser.PasswordSalt = passwordSalt;
            currentUser.UserRole = userDto.UserRole;

            _unitOfWork.UserRepository.Create(currentUser);
            _unitOfWork.Save();

            return Ok(currentUser); 
        }

        [HttpPost("login")]
        [EnableCors("default")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            if (!_unitOfWork.UserRepository.IsUserExists(loginDto.Username))
            {
                var errorResponse = new { message = "Invalid password or username" };
                return BadRequest(errorResponse);
            }
            var currentUser = _unitOfWork.UserRepository.GetUserByUsername(loginDto.Username);

            if(!VerifyPassword(loginDto.Password, currentUser.PasswordHash, currentUser.PasswordSalt))
            {
                var errorResponse = new { message = "Invalid password or username" };
                return BadRequest(errorResponse);
            }
            var token = CreateToken(currentUser);
            return Ok(new UserTokenDto() { Token = token });
        }

        private string CreateToken(User user)
        {
            var listClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.
                GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: listClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
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
