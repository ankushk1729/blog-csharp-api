using Microsoft.EntityFrameworkCore;
using SM.Entities;
namespace SM.Data
{
    public class ApiDBContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Comment> Comments { get; set; }

         public ApiDBContext(DbContextOptions<ApiDBContext> options)
        : base(options){}
    }
}