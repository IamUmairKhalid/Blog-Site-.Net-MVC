using Microsoft.EntityFrameworkCore;

namespace BlogSite.Data
{
    public class AppDbContext : DbContext
    {
        AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            
        }
    }
}
