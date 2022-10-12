using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SM.Data;
using SM.Utils;
using SM.Dtos;
using SM.Entities;

namespace SM.Controllers
{   

    [ApiController]
    [Route("blogs")]
    public class BlogsController : ControllerBase
    {
        private ApiDBContext _dbContext { get; set; }

        public BlogsController(){
            this._dbContext = new ApiDBContext();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBlogs() {
            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);
            if(!AuthUtil.AuthorizePermissions(user!)){
                return Unauthorized();
            }

            return Ok(BlogUtil.AttachUsersToBlogs(_dbContext, _dbContext.Blogs));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBlog(Guid id){
            var blog = BlogUtil.AttachUserToBlog(_dbContext, id);
            if(blog is null) {
                return NotFound();
            }

            return Ok(blog);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateBlog(CreateBlogDto blogData) {
            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            var newBlog = new Blog() {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now,
                Content = blogData.Content,
                Cover = blogData.Cover,
                NumLikes = 0,
                User = user,
                UserId = user.UserId
            };

            _dbContext.Blogs.Add(newBlog);
            _dbContext.SaveChanges();

            return Ok(newBlog);
        }

    }
}