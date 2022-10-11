using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SM.Dtos;
using SM.Data;
using SM.Entities;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace SM.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        private ApiDBContext _dbContext { get; set; }
        private IConfiguration _config;

        public AuthController(IConfiguration configuration){
            this._dbContext = new ApiDBContext();
            this._config = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDto loginUserDto){
            var existingUser = _dbContext.Users.FirstOrDefault(user => user.Email == loginUserDto.Email && user.Password == loginUserDto.Password);

            if(existingUser is null) {
                return NotFound("No user exists with that credentials");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Email, existingUser.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);

        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto registerUserDto){
            var userExists = _dbContext.Users.FirstOrDefault(user => user.Email == registerUserDto.Email);

            if(userExists is not null) {
                return BadRequest("User already exists, please login");
            }

           var user = new User() {
            UserId = Guid.NewGuid(),
            Email = registerUserDto.Email,
            Password = registerUserDto.Password,
            Username = registerUserDto.Username,
            CreatedAt = DateTimeOffset.Now
           };

           _dbContext.Users.Add(user);
           _dbContext.SaveChanges();

            return Ok(user);

        }

    }
}