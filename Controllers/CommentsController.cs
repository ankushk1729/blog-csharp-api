using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SM.Data;
using SM.Utils;
using SM.Dtos;
using SM.Entities;

namespace SM.Controllers
{
    [ApiController]
    [Route("comments")]
    public class CommentsController : ControllerBase
    {

        private ApiDBContext _dbContext;

        public CommentsController() {
            this._dbContext = new ApiDBContext();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetComments() {
            var comments = CommentUtil.AttachUsersToComments(_dbContext, _dbContext.Comments);

            return Ok(comments.Select(comment => comment.AsDto()));
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetComment(Guid id) {
            var commentData = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

            if(commentData is null) {
                return NotFound("No such comment exists with id : " + id);
            }

            var comment = CommentUtil.AttachUserToComment(_dbContext, commentData!.Id);

            return Ok(comment.AsDto());
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateComment(CreateCommentDto commentData) {
            var blog = _dbContext.Blogs.FirstOrDefault(b => b.Id == commentData.BlogId);

            if(blog is null) {
                return NotFound("No such blog exists with id : " + commentData.BlogId);
            }

            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            var comment = new Comment() {
                Id = Guid.NewGuid(), 
                Text = commentData.Text,
                User = user, 
                UserId = user.UserId,
                Blog = blog,
                BlogId = blog.Id,
                CreatedAt = DateTimeOffset.Now,
            };

            _dbContext.Comments.Add(comment);

            _dbContext.SaveChanges();

            var comm = CommentUtil.AttachUserToComment(_dbContext, comment.Id);

            return Ok(comm.AsDto());

        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteComment(Guid id) {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);

            if(comment is null) {
                return NotFound("No comment with id : " + id);
            }

            var user = AuthUtil.GetCurrentUser(_dbContext, HttpContext);

            if(!AuthUtil.AuthorizePermissions(_dbContext, user, comment)) {
                return Unauthorized();
            }

            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();

            return Ok("Comment deleted");

        }
    }  
}