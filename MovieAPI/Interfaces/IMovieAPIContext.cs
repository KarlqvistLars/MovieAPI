using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Interfaces
{
    public interface IMovieAPIContext
    {
        DbSet<Movie> Movies { get; set; }
        DbSet<MovieDetails> MovieDetails { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Actor> Actors { get; set; }
        DbSet<Review> Reviews { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =
        default);
    }
}
