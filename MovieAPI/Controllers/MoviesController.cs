using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //private readonly MovieAPIContext _context;
    //public MoviesController(MovieAPIContext context)
    //{
    //    _context = context;
    //}

    private readonly IMovieService _movieService;

    // Ändra här: Ta emot interfacet istället för MovieAPIContext
    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movie
    [HttpGet]
    public async Task<ActionResult<ICollection<MovieDto>>> GetMovies()
    {
        var movies = await _movieService.GetMovies();
        return Ok(movies);
    }

    // GET: api/movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovieById(int id)
    {

        var movie = await _movieService.GetMovieById(id);

        if (movie == null)
        {
            return NotFound();
        }
        return Ok(movie);
    }

    //    // GET: api/movie/5/reviews
    //    [HttpGet("{id}/reviews")]
    //    public async Task<ActionResult<Movie>> GetMovieReviews(int id)
    //    {
    //        var movie = await _context.Movies
    //            .Include(m => m.Reviews)   // Hämta Review för id
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (movie == null)
    //        {
    //            return NotFound();
    //        }
    //        var reviewDtos = movie.Reviews?.Select(r => new ReviewDto { Title = movie.Title, ReviewerName = r.ReviewerName, Rating = r.Rating, Comment = r.Comment }).ToList();
    //        return Ok(reviewDtos);
    //    }

    //    // GET: api/movie/5/details
    //    [HttpGet("{id}/details")]
    //    public async Task<ActionResult<Movie>> GetMovieDetails(int id)
    //    {
    //        var movie = await _context.Movies
    //            .Include(m => m.Details)   // Hämtar MovieDetails
    //            .FirstOrDefaultAsync(m => m.Id == id);

    //        if (movie == null)
    //        {
    //            return NotFound();
    //        }
    //        var details = movie?.Details != null ? new MovieDetailsDto {
    //            Title = movie?.Title,
    //            Synopsis = movie?.Details.Synopsis,
    //            Language = movie?.Details.Language,
    //            Budget = movie?.Details.Budget
    //        } : null;

    //        return Ok(details);
    //    }

    //    // PUT: api/movie/2
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutMovie(int id, MovieDto movieDto)
    //    {
    //        // Hämta befintlig film inklusive dess relaterade data
    //        var existingMovie = await _context.Movies
    //            .Include(m => m.Details)
    //            .Include(m => m.Actors)
    //            .Include(m => m.Genres)
    //            .FirstOrDefaultAsync(m => m.Id == id);

    //        if (existingMovie == null)
    //        {
    //            return NotFound();
    //        }

    //        // Uppdatera enkla fält
    //        existingMovie.Title = movieDto.Title;
    //        existingMovie.Year = movieDto.Year;
    //        existingMovie.Duration = movieDto.Duration;

    //        // Uppdatera detaljer
    //        if (existingMovie.Details != null && movieDto.Details != null)
    //        {
    //            existingMovie.Details.Synopsis = movieDto.Details.Synopsis;
    //            existingMovie.Details.Language = movieDto.Details.Language;
    //            existingMovie.Details.Budget = movieDto.Details.Budget;
    //        }

    //        // Hantera listor (Actors/Genres)
    //        _context.Actors.RemoveRange(existingMovie.Actors);
    //        existingMovie.Actors = movieDto.Actors.Select(a => new Actor { Name = a.Name }).ToList();

    //        _context.Genres.RemoveRange(existingMovie.Genres);
    //        existingMovie.Genres = movieDto.Genres.Select(g => new Genre { GenreName = g.GenreName }).ToList();

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        } catch (DbUpdateConcurrencyException)
    //        {
    //            if (!MovieExists(id)) return NotFound();
    //            throw;
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/movie
    //    [HttpPost]
    //    public async Task<ActionResult<Movie>> PostMovie(MovieDto movieDto)
    //    {
    //        // Skapa en ny Movie-entitet baserat på DTO:n och spara de grundläggande fälten, lägg sedan till relaterade data (Details, Actors, Genres, Reviews)
    //        var movie = new Movie { Title = movieDto.Title, Year = movieDto.Year, Duration = movieDto.Duration };
    //        // Spara MovieDetails
    //        if (movieDto.Details != null)
    //        {
    //            movie.Details = new MovieDetails {
    //                Synopsis = movieDto.Details.Synopsis,
    //                Language = movieDto.Details.Language,
    //                Budget = movieDto.Details.Budget
    //            };
    //        }
    //        // Spara Actors
    //        if (movieDto.Actors != null)
    //        {
    //            foreach (var actorDto in movieDto.Actors)
    //            {
    //                movie.Actors.Add(new Actor { Name = actorDto.Name, BirthYear = actorDto.BirthYear });
    //            }
    //        }
    //        // Spara Genre
    //        if (movieDto.Genres != null)
    //        {
    //            foreach (var genreDto in movieDto.Genres)
    //            {
    //                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreName == genreDto.GenreName);
    //                movie.Genres.Add(existingGenre ?? new Genre { GenreName = genreDto.GenreName });
    //            }
    //        }
    //        // Spara Review
    //        if (movieDto.Reviews != null)
    //        {
    //            foreach (var reviewDto in movieDto.Reviews)
    //            {
    //                movie.Reviews.Add(new Review { ReviewerName = reviewDto.ReviewerName, Rating = reviewDto.Rating, Comment = reviewDto.Comment });
    //            }
    //        }
    //        // Lägg till den nya filmen i databasen
    //        _context.Movies.Add(movie);
    //        // Spara i en try catch för att hantera eventuella datakonflikter.
    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        } catch (DbUpdateException)
    //        {
    //            return Conflict("Ett fel uppstod vid sparande, troligen en konflikt med existerande data.");
    //        }
    //        // Returnera resultatet med CreatedAtAction som pekar på GetMovie filmdatat.
    //        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    //    }

    //    // DELETE: api/movie/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteMovie(int? id)
    //    {
    //        var movie = await _context.Movies.FindAsync(id);
    //        if (movie == null)
    //        {
    //            return NotFound();
    //        }
    //        var details = await _context.MovieDetails.FindAsync(movie.DetailsId);
    //        if (details != null)
    //        {
    //            _context.MovieDetails.Remove(details);
    //        }
    //        var review = await _context.Reviews.Where(r => r.MovieId == id).ToListAsync();
    //        if (review != null)
    //        {
    //            _context.Reviews.RemoveRange(review);
    //        }

    //        _context.Movies.Remove(movie);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool MovieExists(int? id)
    //    {
    //        return _context.Movies.Any(e => e.Id == id);
    //    }


}
