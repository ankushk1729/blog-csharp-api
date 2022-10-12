using SM.Dtos;
using SM.Entities;
namespace SM
{
    public static class Extensions {
        public static BlogDto AsDto(this Blog blog){
            return new BlogDto() {Id = blog.Id, Content = blog.Content, Cover = blog.Cover, CreatedAt = blog.CreatedAt, UserId = blog.UserId, NumLikes = blog.NumLikes, User = new UserDto() { UserId = blog.User.UserId, Username = blog.User.Username, Email = blog.User.Email, CreatedAt = blog.User.CreatedAt, Role = blog.User.Role}};
        }

        public static UserDto AsDto(this User user) {
            return new UserDto() { UserId = user.UserId, Username = user.Username, Email = user.Email, Role = user.Role, CreatedAt = user.CreatedAt};
        }
    }
}