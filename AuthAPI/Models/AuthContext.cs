using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
    }
   
}
