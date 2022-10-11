using Microsoft.AspNetCore.Mvc;
using SM.Entities;
using SM.Data;
namespace SM.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {

        private IDBContext _dbContext { get; set; }

        public UsersController(IDBContext dBContext){
            this._dbContext = dBContext;
        }


        [HttpGet]
        public IActionResult GetUsers(){
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