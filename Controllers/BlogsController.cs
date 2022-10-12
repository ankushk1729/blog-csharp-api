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

            var blogsData = BlogUtil.AttachUsersToBlogs(_dbContext, _dbContext.Blogs);

            var blogs = blogsData.Select(blog => blog.AsDto());

            return Ok(blogs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBlog(Guid id){
            var blogData = BlogUtil.AttachUserToBlog(_dbContext, id);
            if(blogData is null) {
                return NotFound();
            }

            var blog = blogData.AsDto();

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

            return Ok(newBlog.AsDto());
        }

    }
}