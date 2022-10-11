using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SM.Data;
using SM.Utils;
using System.Security.Claims;

namespace SM.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {

        private ApiDBContext _dbContext { get; set; }

        public UsersController(){
            this._dbContext = new ApiDBContext();
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetUsers(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userEmail = "";
            if(identity != null) {
                userEmail = identity.Claims.FirstOrDefault()!.Value;    
            }
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == userEmail);
            if(!AuthUtil.AuthorizePermissions(user!)){
                return Unauthorized();
            }

            return Ok(_dbContext.Users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id){
            var user = _dbContext.Users.FirstOrDefault(user => user.UserId == id);

            if(user is null) {
                return NotFound("No such user with id : " + id);
            }

            return Ok(user);
        }
    }
}