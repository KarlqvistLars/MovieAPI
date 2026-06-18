using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;


namespace MovieAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieAPIContext _db;
        public MovieService(IMovieAPIContext db)
        {
            _db = db;
        }


        public async Task<ActionResult<ICollection<MovieDto>>> GetMovies()
        {
            // Vi använder .Include för att hämta den relaterade datan
            var movies = await _db.Movies
                .Include(m => m.Details)   // Hämtar MovieDetails
                .Include(m => m.Actors)    // Hämtar listan av Actors
                .Include(m => m.Genres)    // Hämtar listan av Genres
                .Include(m => m.Reviews)   // Hämtar listan av Reviews
                .ToListAsync();

            // Mappa entiteter till DTO:er
            var movieDtos = movies.Select(m => new MovieDto {
                Id = m.Id,
                Title = m.Title,
                Year = m.Year,
                Duration = m.Duration,
                Details = m.Details != null ? new MovieDetailsDto {
                    Synopsis = m.Details.Synopsis,
                    Language = m.Details.Language,
                    Budget = m.Details.Budget
                } : null,
                Actors = m.Actors?.Select(a => new ActorDto { Name = a.Name, BirthYear = a.BirthYear }).ToList(),
                Genres = m.Genres?.Select(g => new GenreDto { GenreName = g.GenreName }).ToList(),
                Reviews = m.Reviews?.Select(r => new ReviewDto { ReviewerName = r.ReviewerName, Rating = r.Rating, Comment = r.Comment }).ToList()
            }).ToList();
            return movieDtos;
        }

        //public async Task<ActionResult<Movie>> GetMovieById(int id)
        //{
        //    return await _db.Movies.FindAsync(id);
        //}

        public async Task<MovieDto?> GetMovieById(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Details)   // Hämtar MovieDetails
                .Include(m => m.Actors)    // Hämtar listan av Actors
                .Include(m => m.Genres)    // Hämtar listan av Genres
                .Include(m => m.Reviews)   // Hämtar listan av Reviews
                .FirstOrDefaultAsync(m => m.Id == id);

            var movieDto = new MovieDto {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Duration = movie.Duration,
                Details = movie.Details != null ? new MovieDetailsDto {
                    Synopsis = movie.Details.Synopsis,
                    Language = movie.Details.Language,
                    Budget = movie.Details.Budget
                } : null,
                Actors = movie.Actors?.Select(a => new ActorDto { Name = a.Name, BirthYear = a.BirthYear }).ToList(),
                Genres = movie.Genres?.Select(g => new GenreDto { GenreName = g.GenreName }).ToList(),
                Reviews = movie.Reviews?.Select(r => new ReviewDto { ReviewerName = r.ReviewerName, Rating = r.Rating, Comment = r.Comment }).ToList()
            };

            return movieDto;
        }
    }
}

