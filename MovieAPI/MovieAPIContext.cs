using Microsoft.EntityFrameworkCore;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<MovieAPI.Models.Movie> Movies { get; set; } = default!;
}
