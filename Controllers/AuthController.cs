using Microsoft.AspNetCore.Mvc;
using SM.Dtos;
using SM.Data;
using SM.Entities;
using SM.Utils;

namespace SM.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        private ApiDBContext _dbContext { get; set; }
        private IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            this._dbContext = new ApiDBContext();
            this._config = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDto loginUserDto)
        {

            var existingUser = _dbContext.Users.FirstOrDefault(user => user.Email == loginUserDto.Email);

            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, existingUser.Password);

            if (existingUser is null || !isPasswordCorrect)
            {
                return NotFound("Invalid credentials");
            }

            var jwt = AuthUtil.CreateToken(_config, existingUser.Email);

            return Ok(new { token = jwt });

        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto registerUserDto)
        {
            var userExists = _dbContext.Users.FirstOrDefault(user => user.Email == registerUserDto.Email || user.Username == registerUserDto.Username);

            if (userExists is not null)
            {
                return BadRequest("User already exists, please login");
            }

            var password = registerUserDto.Password;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Email = registerUserDto.Email,
                Password = hashedPassword,
                Username = registerUserDto.Username,
                CreatedAt = DateTimeOffset.Now,
            };

            _dbContext.Users.Add(user);

            _dbContext.SaveChanges();
            
            UserUtil.CreateUserRoleMapping(_dbContext, user, "user");


            return Ok(user);

        }

    }
}