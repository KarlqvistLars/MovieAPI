using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Interfaces;
using MovieAPI.Login;
using MovieAPI.Models;

namespace MovieAPI.Data;

public class MovieAPIContext : IdentityDbContext<AppUser>, IMovieAPIContext
{
    public MovieAPIContext(DbContextOptions<MovieAPIContext> options) : base(options)
    {
    }
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<Actor> Actors { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
}
