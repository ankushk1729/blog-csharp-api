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
            if(!AuthUtil.AuthorizePermissions(_dbContext, user!)){
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

        [HttpGet("users/{id}")]
        [Authorize]
        public IActionResult GetUserBlogs(Guid id) {
            var userBlogsData = _dbContext.Blogs.Where(blog => blog.UserId == id);

            List<object> userBlogs = new List<object>();

            foreach(Blog b in userBlogsData) {
                userBlogs.Add(new { Id = b.Id, Title = b.Title, Content = b.Content, Cover = b.Cover, UserId = b.UserId, CreatedAt = b.CreatedAt});
            }
            return Ok(userBlogs);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateBlog(CreateBlogDto blogData) {
            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            var newBlog = new Blog() {
                Id = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.Now,
                Title = blogData.Title,
                Content = blogData.Content,
                Cover = blogData.Cover,
                User = user,
                UserId = user.UserId
            };

            _dbContext.Blogs.Add(newBlog);
            _dbContext.SaveChanges();

            return Ok(newBlog.AsDto());
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteBlog(Guid id) {
            var blog = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);

            if(blog is null) {
                return NotFound();
            }

            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            if(!AuthUtil.AuthorizePermissions(_dbContext, user, blog)) {
                return Unauthorized();
            }

            _dbContext.Blogs.Remove(blog);

            _dbContext.SaveChanges();

            return Ok("Blog deleted successfully");
        }

        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult UpdateBlog(Guid id, UpdateBlogDto updateBlogData) {
            var blog = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);

            if(blog is null) {
                return NotFound();
            }

            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            if(!AuthUtil.AuthorizePermissions(_dbContext, user, blog)) {
                return Unauthorized();
            }

            if(updateBlogData.Title != null) {
                blog.Title = updateBlogData.Title;
            }

            if(updateBlogData.Content != null) {
                blog.Content = updateBlogData.Content;
            }

            if(updateBlogData.Cover != null) {
                blog.Cover = updateBlogData.Cover;
            }
            _dbContext.SaveChanges();

            return Ok("Blog updated successfully");
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult SearchBlogs(string q) {
            var blogs = new List<Blog>();
            
            blogs.AddRange(_dbContext.Blogs.Where(blog => blog.Title.Contains(q)).Select(blog => blog));
            blogs.AddRange(_dbContext.Blogs.Where(blog => blog.Content.Contains(q)).Select(blog => blog));

            return Ok(blogs);
        }


    }
}