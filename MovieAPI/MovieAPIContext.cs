using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<Movie> Movie { get; set; } = default!;
    public DbSet<Genre> Genre { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Review> Review { get; set; } = default!;
    public DbSet<Actor> Actor { get; set; } = default!;
    public DbSet<MovieActor> MovieActor { get; set; } = default!;
}
