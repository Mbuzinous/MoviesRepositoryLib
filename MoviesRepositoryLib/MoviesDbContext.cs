using Microsoft.EntityFrameworkCore;
// NuGet package Microsoft.EntityFrameworkCore.SqlServer

namespace MoviesRepositoryLib
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(
            DbContextOptions<MoviesDbContext> options) : 
            base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}