using Microsoft.EntityFrameworkCore;
using SM.Entities;
namespace SM.Data
{
    public class ApiDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=localhost;Initial Catalog=sm;Integrated Security=True");
        }
    }
}