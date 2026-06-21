using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    private readonly IMovieService _movieService;

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

    // GET: api/movie/5/reviews
    [HttpGet("{id}/reviews")]
    public async Task<ActionResult<MovieDto>> GetMovieReviews(int id)
    {
        var movie = await _movieService.GetMovieReviews(id);
        if (movie == null)
        {
            return NotFound();
        }
        var reviewDtos = movie.Reviews?.Select(r => new ReviewDto { Title = movie.Title, ReviewerName = r.ReviewerName, Rating = r.Rating, Comment = r.Comment }).ToList();
        return Ok(reviewDtos);
    }

    // GET: api/movie/5/details
    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieDto>> GetMovieDetails(int id)
    {
        var movie = await _movieService.GetMovieDetails(id);

        if (movie == null)
        {
            return NotFound();
        }
        var details = movie?.Details != null ? new MovieDetailsDto {
            Title = movie.Title,
            Synopsis = movie.Details.Synopsis,
            Language = movie.Details.Language,
            Budget = movie.Details.Budget
        } : null;

        return Ok(details);
    }

    // PUT: api/movie/2
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int id, MovieDto movieDto)
    {
        return await _movieService.PutMovie(id, movieDto);
    }

    //POST: api/movie
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie([FromBody] MovieDto movieDto)
    {
        // Skapa en ny Movie baserat på DTO:n och spara de grundläggande fälten.
        return await _movieService.PostMovie(movieDto);
    }

    // DELETE: api/movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        return await _movieService.DeleteMovie(id);
    }
}
