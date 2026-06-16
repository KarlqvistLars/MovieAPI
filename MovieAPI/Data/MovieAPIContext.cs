using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Data;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<MovieAPI.Models.Movie> Movies { get; set; } = default!;
}
