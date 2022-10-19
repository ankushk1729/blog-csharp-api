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
    [Route("comments")]
    public class CommentsController : ControllerBase
    {

        private ApiDBContext _dbContext;

        public IMapper _mapper;

        public CommentsController(IMapper mapper, ApiDBContext dBContext)
        {
            this._dbContext = dBContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetComments()
        {
            try
            {
                var comments = CommentUtil.AttachUsersToComments(_dbContext, _dbContext.Comments);

                return Ok(comments.Select(comment => comment.AsDto()));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetComment(Guid id)
        {
            try
            {
                var commentData = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

                if (commentData is null)
                {
                    return NotFound("No such comment exists with id : " + id);
                }

                var comment = CommentUtil.AttachUserToComment(_dbContext, commentData!.Id);

                return Ok(comment.AsDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("users/{name}")]
        [Authorize]
        public IActionResult GetUserComments(string name)
        {
            try
            {
                var currentUser = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                var commentsData = _dbContext.Comments.Where(c => c.User.Username == name);

                List<object> comments = new List<object>();
                foreach (Comment c in commentsData)
                {
                    comments.Add(new { Id = c.Id, Text = c.Text, BlogId = c.BlogId, CreatedAt = c.CreatedAt });
                }

                return Ok(comments);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateComment(CreateCommentDto commentData)
        {
            try
            {
                var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == commentData.BlogId);

                if (blog is null)
                {
                    return NotFound("No such blog exists with id : " + commentData.BlogId);
                }

                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                var comment = _mapper.Map<Comment>(commentData);
                comment.Id = Guid.NewGuid();
                comment.User = user;
                comment.UserId = user.UserId;
                comment.Blog = blog;
                comment.CreatedAt = DateTimeOffset.Now;

                _dbContext.Comments.Add(comment);

                _dbContext.SaveChanges();

                var comm = CommentUtil.AttachUserToComment(_dbContext, comment.Id);

                return Ok(comm.AsDto());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteComment(Guid id)
        {
            try
            {
                var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

                if (comment is null)
                {
                    return NotFound("No comment with id : " + id);
                }

                var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

                if (!AuthUtil.AuthorizePermissions(_dbContext, user, comment))
                {
                    return Unauthorized();
                }

                _dbContext.Comments.Remove(comment);
                _dbContext.SaveChanges();

                return Ok("Comment deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}