using SM.Data;
using SM.Entities;
namespace SM.Utils
{
    public static class BlogUtil
    {
        public static Blog AttachUserToBlog(ApiDBContext dBContext, Guid id)
        {
            return dBContext.Blogs.Join(
                dBContext.Users,
                 blog => blog.UserId, user => user.UserId,
                  (blog, user) => new Blog() { Id = blog.Id, Content = blog.Content, Cover = blog.Cover, User = new() { UserId = user.UserId, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt, Role = user.Role }, UserId = blog.UserId, CreatedAt = blog.CreatedAt }).FirstOrDefault(blog => blog.Id == id)!;
        }

        public static IEnumerable<Blog> AttachUsersToBlogs(ApiDBContext dBContext, IEnumerable<Blog> blogs)
        {
            return dBContext.Blogs.Join(
                dBContext.Users,
                 blog => blog.UserId, user => user.UserId,
                  (blog, user) => new Blog() { Id = blog.Id, Content = blog.Content, Cover = blog.Cover, User = new() { UserId = user.UserId, Username = user.Username, Email = user.Email, CreatedAt = user.CreatedAt, Role = user.Role }, UserId = blog.UserId, CreatedAt = blog.CreatedAt })!;
        }
    }
}