using Microsoft.EntityFrameworkCore;
using ChatAppBackEnd.Model;

namespace ChatAppBackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}