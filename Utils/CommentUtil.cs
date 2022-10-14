using SM.Data;
using SM.Entities;

namespace SM.Utils
{
    public static class CommentUtil
    {
        public static Comment AttachUserToComment(ApiDBContext dBContext, Guid CommentId)
        {
            return dBContext.Comments.Join(dBContext.Users, comment => comment.UserId, user => user.UserId, (comment, user) => new Comment() { Id = comment.Id, Text = comment.Text, Blog = comment.Blog, BlogId = comment.BlogId, UserId = comment.UserId, CreatedAt = comment.CreatedAt, User = new User() { UserId = user.UserId, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt }}).FirstOrDefault(comment => comment.Id == CommentId)!;
        }

        public static IEnumerable<Comment> AttachUsersToComments(ApiDBContext dBContext, IEnumerable<Comment> blogs) {
            return dBContext.Comments.Join(dBContext.Users, comment => comment.UserId, user => user.UserId, (comment, user) => new Comment() { Id = comment.Id, Text = comment.Text, Blog = comment.Blog, BlogId = comment.BlogId, UserId = comment.UserId, CreatedAt = comment.CreatedAt, User = new User() { UserId = user.UserId, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt }})!;
        }
    }
}