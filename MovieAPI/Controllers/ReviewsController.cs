using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;

// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
[Route("api/[controller]")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // GET: api/review
    [HttpGet]
    public async Task<ActionResult<ICollection<ReviewDto>>> GetReview()
    {
        var reviews = await _reviewService.GetReviews();
        if (!reviews.Any()) { return NotFound(); }
        return Ok(reviews);
    }

    // GET: api/review/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReview(int id)
    {
        var review = await _reviewService.GetReview(id);
        if (review == null) { return NotFound(); }
        return Ok(review);
    }

    // PUT: api/review/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReview(int id, UpdateReviewDto review)
    {
        bool updated = await _reviewService.PutReview(id, review);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/review
    [HttpPost("{movieId:int}")]
    public async Task<ActionResult<ReviewDto>> PostReview(
        int movieId,
        [FromBody] ReviewDto reviewDto)
    {
        var result = await _reviewService.PostReview(movieId, reviewDto);
        if (result == null) { return NotFound(); }
        return CreatedAtAction(
            nameof(GetReview),
            new { id = result.Id },
            result);
    }

    // DELETE: api/review/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int? id)
    {
        var result = await _reviewService.DeleteReview(id);
        return result;
    }

}
