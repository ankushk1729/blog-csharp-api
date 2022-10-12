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

        public UsersController(){
            this._dbContext = new ApiDBContext();
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetUsers(){
            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);
            if(!AuthUtil.AuthorizePermissions(user!)){
                return Unauthorized();
            }

            return Ok(_dbContext.Users.Select(user => user.AsDto()));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUser(Guid id){
            var user = _dbContext.Users.FirstOrDefault(user => user.UserId == id);

            if(user is null) {
                return NotFound("No such user with id : " + id);
            }

            return Ok(user.AsDto());
        }
    }
}