using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;
using MovieAPI.Models;


namespace MovieAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieAPIContext _db;
        public MovieService(IMovieAPIContext db)
        {
            _db = db;
        }

        public async Task<ICollection<MovieDto>?> GetMovies()
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

        public async Task<MovieDto?> GetMovieById(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Details)   // Hämtar MovieDetails
                .Include(m => m.Actors)    // Hämtar listan av Actors
                .Include(m => m.Genres)    // Hämtar listan av Genres
                .Include(m => m.Reviews)   // Hämtar listan av Reviews
                .FirstOrDefaultAsync(m => m.Id == id);

            var movieDto = new MovieDto {
                Id = movie?.Id ?? 0,
                Title = movie?.Title ?? string.Empty,
                Year = movie?.Year ?? string.Empty,
                Duration = movie?.Duration ?? string.Empty,
                Details = movie?.Details is { } details
                ? new MovieDetailsDto {
                    Synopsis = details.Synopsis ?? string.Empty,
                    Language = details.Language ?? string.Empty,
                    Budget = details.Budget ?? string.Empty
                } : null,
                Actors = movie?.Actors?.Select(a => new ActorDto { Name = a.Name ?? string.Empty, BirthYear = a.BirthYear }).ToList(),
                Genres = movie?.Genres?.Select(g => new GenreDto { GenreName = g.GenreName ?? string.Empty }).ToList(),
                Reviews = movie?.Reviews?.Select(r => new ReviewDto { ReviewerName = r.ReviewerName ?? string.Empty, Rating = r.Rating, Comment = r.Comment ?? string.Empty }).ToList()
            };
            return movieDto;
        }

        public async Task<MovieDto?> GetMovieReviews(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Reviews)   // Hämta Review för id
                .FirstOrDefaultAsync(m => m.Id == id);

            var reviewDtos = new MovieDto {
                Id = movie?.Id != null ? movie.Id : 0,
                Title = movie?.Title ?? string.Empty,
                Reviews = movie?.Reviews?.Select(r => new ReviewDto { ReviewerName = r.ReviewerName ?? string.Empty, Rating = r.Rating, Comment = r.Comment ?? string.Empty }).ToList()
            };
            return reviewDtos;
        }

        public async Task<MovieDto?> GetMovieDetails(int id)
        {
            var movie = await _db.Movies
                .Include(m => m.Details)   // Hämtar MovieDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return null;
            }
            var movieDetails = new MovieDto {
                Id = movie.Id,
                Title = movie.Title,
                Details = movie.Details is { } details
                    ? new MovieDetailsDto {
                        Synopsis = details.Synopsis ?? string.Empty,
                        Language = details.Language ?? string.Empty,
                        Budget = details.Budget ?? string.Empty
                    }
                    : null
            };
            return movieDetails;
        }

        public async Task<IActionResult> PutMovie(int id, MovieDto movieDto)
        {
            // Hämta befintlig film inklusive dess relaterade data
            var existingMovie = await _db.Movies
                .Include(m => m.Details)
                .Include(m => m.Actors)
                .Include(m => m.Genres)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMovie == null) { return new NotFoundResult(); }
            if (movieDto == null) { return new BadRequestResult(); }

            // Uppdatera enkla fält
            existingMovie.Title = movieDto.Title;
            existingMovie.Year = movieDto.Year;
            existingMovie.Duration = movieDto.Duration;
            // Uppdatera detaljer
            if (movieDto.Details != null)
            {
                if (existingMovie.Details != null && movieDto?.Details != null)
                {
                    existingMovie.Details.Synopsis = movieDto.Details.Synopsis;
                    existingMovie.Details.Language = movieDto.Details.Language;
                    existingMovie.Details.Budget = movieDto.Details.Budget;
                }
            }

            // Hantera listor (Actors/Genres)
            if (existingMovie.Genres != null) { _db.Genres.RemoveRange(existingMovie.Genres); }
            existingMovie.Genres = movieDto!.Genres?.Select(g => new Genre { GenreName = g.GenreName })
                .ToList() ?? new List<Genre>();

            if (existingMovie.Actors != null) { _db.Actors.RemoveRange(existingMovie.Actors); }
            existingMovie.Actors = movieDto!.Actors?.Select(a => new Actor { Name = a.Name })
                .ToList() ?? new List<Actor>();

            if (existingMovie.Reviews != null) { _db.Reviews.RemoveRange(existingMovie.Reviews); }
            existingMovie.Reviews = movieDto!.Reviews?.Select(r => new Review { ReviewerName = r.ReviewerName, Rating = r.Rating, Comment = r.Comment })
                .ToList() ?? new List<Review>();

            await _db.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<ActionResult<MovieDto>> PostMovie(MovieDto movieDto)
        {
            // Skapa en ny Movie-entitet baserat på DTO:n och spara de grundläggande fälten, lägg sedan till relaterade data (Details, Actors, Genres, Reviews)
            var movie = new Movie { Title = movieDto.Title, Year = movieDto.Year, Duration = movieDto.Duration };
            // Spara MovieDetails
            if (movieDto.Details != null)
            {
                movie.Details = new MovieDetails {
                    Synopsis = movieDto.Details.Synopsis,
                    Language = movieDto.Details.Language,
                    Budget = movieDto.Details.Budget
                };
            }
            // Spara Actors
            if (movieDto.Actors != null)
            {
                foreach (var actorDto in movieDto.Actors)
                {
                    movie.Actors.Add(new Actor { Name = actorDto.Name ?? string.Empty, BirthYear = actorDto.BirthYear ?? string.Empty });
                }
            }
            // Spara Genre
            if (movieDto.Genres != null)
            {
                foreach (var genreDto in movieDto.Genres)
                {
                    var existingGenre = await _db.Genres.FirstOrDefaultAsync(g => g.GenreName == genreDto.GenreName);
                    movie.Genres.Add(existingGenre ?? new Genre { GenreName = genreDto.GenreName });
                }
            }
            // Spara Review
            if (movieDto.Reviews != null)
            {
                foreach (var reviewDto in movieDto.Reviews)
                {
                    movie.Reviews.Add(new Review { ReviewerName = reviewDto.ReviewerName ?? string.Empty, Rating = reviewDto.Rating, Comment = reviewDto.Comment ?? string.Empty });
                }
            }
            // Lägg till den nya filmen i databasen
            _db.Movies.Add(movie);
            // Spara i en try catch för att hantera eventuella datakonflikter.
            try
            {
                await _db.SaveChangesAsync();
            } catch (DbUpdateException)
            {
                return new ConflictObjectResult("Ett fel uppstod vid sparande, troligen en konflikt med existerande data.");
            }
            // Returnera resultatet med CreatedAtAction som pekar på GetMovie filmdatat.
            return new CreatedAtActionResult(nameof(GetMovieById), "Movies", new { id = movie.Id }, movie);
        }

        public async Task<IActionResult> DeleteMovie(int? id)
        {
            var movie = await _db.Movies.FindAsync(id);
            if (movie == null)
            {
                return new NotFoundResult();
            }
            var details = await _db.MovieDetails.FindAsync(movie.DetailsId);
            if (details != null)
            {
                _db.MovieDetails.Remove(details);
            }
            var review = await _db.Reviews.Where(r => r.MovieId == id).ToListAsync();
            if (review != null)
            {
                _db.Reviews.RemoveRange(review);
            }

            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();

            return new NoContentResult();
        }
        private bool MovieExists(int? id)
        {
            return _db.Movies.Any(e => e.Id == id);
        }
    }
}

