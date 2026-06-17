using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.DTOs;
using MovieAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieAPIContext _context;
    public MoviesController(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie()
    {
        // Vi använder .Include för att hämta den relaterade datan
        var movies = await _context.Movies
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
        return Ok(movieDtos);
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Details)   // Hämtar MovieDetails
            .Include(m => m.Actors)    // Hämtar listan av Actors
            .Include(m => m.Genres)    // Hämtar listan av Genres
            .Include(m => m.Reviews)   // Hämtar listan av Reviews
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

#pragma warning disable CS8601 // Possible null reference assignment.
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
#pragma warning restore CS8601 // Possible null reference assignment.
        return Ok(movieDto);
    }

    // GET: api/Movie/5
    [HttpGet("{id}/details")]
    public async Task<ActionResult<Movie>> GetMovieDetails(int id)
    {
        var movie = await _context.Movies
            .Include(m => m.Details)   // Hämtar MovieDetails
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }


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
        };

        return Ok(movieDto);
    }



    // PUT: api/Movie/2
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, MovieDto movieDto)
    {
        // 1. Hämta befintlig film inklusive dess relaterade data
        var existingMovie = await _context.Movies
            .Include(m => m.Details)
            .Include(m => m.Actors)
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        // 2. Uppdatera enkla fält
        existingMovie.Title = movieDto.Title;
        existingMovie.Year = movieDto.Year;
        existingMovie.Duration = movieDto.Duration;

        // 3. Uppdatera detaljer
        if (existingMovie.Details != null && movieDto.Details != null)
        {
            existingMovie.Details.Synopsis = movieDto.Details.Synopsis;
            existingMovie.Details.Language = movieDto.Details.Language;
            existingMovie.Details.Budget = movieDto.Details.Budget;
        }

        // 4. Hantera listor (Actors/Genres)
        // Detta är den svåra biten. Det enklaste sättet är att rensa gamla
        // och lägga till nya, eller att göra en mer avancerad diff-logik.
        // Exempel på "rensa och ersätt":
        _context.Actors.RemoveRange(existingMovie.Actors);
        existingMovie.Actors = movieDto.Actors.Select(a => new Actor { Name = a.Name }).ToList();

        _context.Genres.RemoveRange(existingMovie.Genres);
        existingMovie.Genres = movieDto.Genres.Select(g => new Genre { GenreName = g.GenreName }).ToList();

        try
        {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(MovieDto movieDto)
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
                movie.Actors.Add(new Actor { Name = actorDto.Name, BirthYear = actorDto.BirthYear });
            }
        }
        // Spara Genre
        if (movieDto.Genres != null)
        {
            foreach (var genreDto in movieDto.Genres)
            {
                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == genreDto.GenreName);
                movie.Genres.Add(existingGenre ?? new Genre { GenreName = genreDto.GenreName });
            }
        }
        // Spara Review
        if (movieDto.Reviews != null)
        {
            foreach (var reviewDto in movieDto.Reviews)
            {
                movie.Reviews.Add(new Review { ReviewerName = reviewDto.ReviewerName, Rating = reviewDto.Rating, Comment = reviewDto.Comment });
            }
        }
        // Lägg till den nya filmen i databasen
        _context.Movies.Add(movie);
        // Spara i en try catch för att hantera eventuella datakonflikter.
        try
        {
            await _context.SaveChangesAsync();
        } catch (DbUpdateException)
        {
            return Conflict("Ett fel uppstod vid sparande, troligen en konflikt med existerande data.");
        }
        // Returnera resultatet med CreatedAtAction som pekar på GetMovie filmdatat.
        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }


    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        var details = await _context.MovieDetails.FindAsync(movie.DetailsId);
        if (details != null)
        {
            _context.MovieDetails.Remove(details);
        }
        var review = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();
        if (review != null)
        {
            _context.Reviews.RemoveRange(review);
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}
