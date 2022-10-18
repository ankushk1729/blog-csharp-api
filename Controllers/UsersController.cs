using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Data;
using SM.Utils;

namespace SM.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {

        private ApiDBContext _dbContext { get; set; }

        public UsersController(ApiDBContext dBContext)
        {
            this._dbContext = dBContext;
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetUsers()
        {
            try
            {
                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);
                if (!AuthUtil.AuthorizePermissions(_dbContext, user!))
                {
                    return Unauthorized();
                }

                return Ok(_dbContext.Users.Select(user => user.AsDto()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{name}")]
        [Authorize]
        public IActionResult GetUser(string name)
        {
            try
            {
                var user = _dbContext.Users.FirstOrDefault(user => user.Username == name);

                if (user is null)
                {
                    return NotFound("No such user with username : " + name);
                }

                return Ok(user.AsDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}