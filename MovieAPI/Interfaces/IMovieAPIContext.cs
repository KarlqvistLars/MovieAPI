using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        EntityEntry Entry(object entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken =
        default);
    }
}
