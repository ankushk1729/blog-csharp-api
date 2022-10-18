using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SM.Data;
using SM.Utils;
using SM.Dtos;
using SM.Entities;
using AutoMapper;

namespace SM.Controllers
{

    [ApiController]
    [Route("blogs")]
    public class BlogsController : ControllerBase
    {
        private ApiDBContext _dbContext { get; set; }
        private readonly IMapper _mapper;

        public BlogsController(IMapper mapper)
        {
            this._dbContext = new ApiDBContext();
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetBlogs()
        {
            try
            {
                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);
                if (!AuthUtil.AuthorizePermissions(_dbContext, user!))
                {
                    return Unauthorized();
                }

                var blogsData = BlogUtil.AttachUsersToBlogs(_dbContext, _dbContext.Blogs);

                var blogs = blogsData.Select(blog => blog.AsDto());

                return Ok(blogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBlog(Guid id)
        {
            try
            {
                var blogData = BlogUtil.AttachUserToBlog(_dbContext, id);
                if (blogData is null)
                {
                    return NotFound();
                }

                var blog = blogData.AsDto();

                return Ok(blog);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("users/{id}")]
        [Authorize]
        public IActionResult GetUserBlogs(Guid id)
        {
            try
            {
                var userBlogsData = _dbContext.Blogs.Where(blog => blog.UserId == id);

                List<object> userBlogs = new List<object>();

                foreach (Blog b in userBlogsData)
                {
                    userBlogs.Add(new { Id = b.Id, Title = b.Title, Content = b.Content, Cover = b.Cover, UserId = b.UserId, CreatedAt = b.CreatedAt });
                }
                return Ok(userBlogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateBlog(CreateBlogDto blogData)
        {
            try
            {
                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                var newBlog = _mapper.Map<Blog>(blogData);
                newBlog.Id = Guid.NewGuid();
                newBlog.User = user;
                newBlog.UserId = user.UserId;
                newBlog.CreatedAt = DateTimeOffset.Now;


                _dbContext.Blogs.Add(newBlog);
                _dbContext.SaveChanges();

                return Ok(newBlog.AsDto());
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteBlog(Guid id)
        {
            try
            {
                var blog = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);

                if (blog is null)
                {
                    return NotFound();
                }

                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                if (!AuthUtil.AuthorizePermissions(_dbContext, user, blog))
                {
                    return Unauthorized();
                }

                _dbContext.Blogs.Remove(blog);

                _dbContext.SaveChanges();

                return Ok("Blog deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult UpdateBlog(Guid id, UpdateBlogDto updateBlogData)
        {
            try
            {
                var blog = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);

                if (blog is null)
                {
                    return NotFound();
                }

                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                if (!AuthUtil.AuthorizePermissions(_dbContext, user, blog))
                {
                    return Unauthorized();
                }

                if (updateBlogData.Title != null)
                {
                    blog.Title = updateBlogData.Title;
                }

                if (updateBlogData.Content != null)
                {
                    blog.Content = updateBlogData.Content;
                }

                if (updateBlogData.Cover != null)
                {
                    blog.Cover = updateBlogData.Cover;
                }
                _dbContext.SaveChanges();

                return Ok("Blog updated successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        [Authorize]
        public IActionResult SearchBlogs(string q)
        {
            try
            {
                var blogs = new List<Blog>();

                blogs.AddRange(_dbContext.Blogs.Where(blog => blog.Title.Contains(q)).Select(blog => blog));
                blogs.AddRange(_dbContext.Blogs.Where(blog => blog.Content.Contains(q)).Select(blog => blog));

                return Ok(blogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}