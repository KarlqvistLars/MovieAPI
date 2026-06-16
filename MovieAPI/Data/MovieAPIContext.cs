using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<MovieAPI.Models.Movie> Movies { get; set; } = default!;
    public DbSet<Genre> Genre { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Review> Review { get; set; } = default!;
    public DbSet<Actor> Actor { get; set; } = default!;
    public DbSet<Temp_MovieActor> MovieActor { get; set; } = default!;
}
