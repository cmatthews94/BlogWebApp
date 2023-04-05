using BlogWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.Database
{
    public class BlogPostDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=.\HYDAELYN;Initial Catalog=BlogWebAppDb;Integrated Security=True;Encrypt=False");
        }
    }
}
