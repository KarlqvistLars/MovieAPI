using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models;

namespace MovieAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesControllerOld : ControllerBase
{
    private readonly MovieAPIContext _context;
    public MoviesControllerOld(MovieAPIContext context)
    {
        _context = context;
    }

    // GET: https://github.com/KarlqvistLars/MovieAPI
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
    {
        return await _context.Movie.ToListAsync();
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movie.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return movie;
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, Movie movie)
    {
        if (id != movie.Id)
        {
            return BadRequest();
        }

        _context.Entry(movie).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            } else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    //public async Task<ActionResult<MovieCreateDto>> PostMovie(MovieCreateDto movie)
    //{
    //    // Genre
    //    var genres = movie.Genres?.Select(g => new Genre { GenreName = g.GenreName }).ToList();
    //    // Review
    //    var newReview = movie.Reviews?.Select(r => new Review { ReviewerName = r.ReviewerName, Comment = r.Comment, Rating = r.Rating }).ToList();
    //    // Actor
    //    var newActor = movie.Actors?.Select(a => new Actor { Name = a.Name, BirthYear = a.BirthYear }).ToList();
    //    // MovieDetails
    //    var newMovieDetails = new MovieDetailDto {
    //        Synopsis = movie.MovieDetails?.Synopsis ?? string.Empty,
    //        Language = movie.MovieDetails?.Language ?? string.Empty,
    //        Budget = movie.MovieDetails?.Budget ?? string.Empty,
    //        Genres = genres,
    //        Reviews = newReview,
    //        Actors = newActor
    //    };
    //    // Movie
    //    var newMovie = new Movie {
    //        Title = movie.Title,
    //        Year = movie.Year,
    //        Duration = movie.Duration,
    //        Details = newMovieDetails
    //    };

    //    _context.Movie.Add(newMovie);

    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction("GetMovie", new { id = newMovie.Id }, newMovie);
    //}

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movie.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movie.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movie.Any(e => e.Id == id);
    }
}
