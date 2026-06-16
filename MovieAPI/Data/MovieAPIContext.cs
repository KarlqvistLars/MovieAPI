using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class MovieAPIContext(DbContextOptions<MovieAPIContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
    public DbSet<Temp_MovieActor> MovieActors { get; set; } = default!;
}
