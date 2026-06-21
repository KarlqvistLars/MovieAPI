using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.DTOs;
using MovieAPI.Interfaces;
using MovieAPI.Models;

namespace MovieAPI.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IMovieAPIContext _db;
        public ReviewService(IMovieAPIContext db)
        {
            _db = db;
        }

        public async Task<ICollection<ReviewDto>> GetReviews()
        {
            var review = await _db.Reviews
            .Include(m => m.Movie)
            .Select(r => new ReviewDto {
                Title = r.Movie.Title,
                ReviewerName = r.ReviewerName,
                Comment = r.Comment,
                Rating = r.Rating,
            }).ToListAsync();
            return review;
        }

        public async Task<ReviewDto> GetReview(int id)
        {
            var review = await _db.Reviews
            .Include(m => m.Movie)
            .Where(r => r.Id == id)
            .Select(r => new ReviewDto {
                Title = r.Movie.Title,
                ReviewerName = r.ReviewerName,
                Comment = r.Comment,
                Rating = r.Rating,
            }).FirstOrDefaultAsync();
            return review;
        }

        public async Task<bool> PutReview(int? id, UpdateReviewDto reviewDto)
        {
            var review = await _db.Reviews.FindAsync(id);

            if (review == null)
            {
                return false;
            }
            review.ReviewerName = reviewDto.ReviewerName;
            review.Comment = reviewDto.Comment;
            review.Rating = reviewDto.Rating;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<ReviewDto?> PostReview(int movieId, ReviewDto reviewDto)
        {
            var movie = await _db.Movies
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return null;
            }

            var review = new Review {
                MovieId = movieId,
                ReviewerName = reviewDto.ReviewerName,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating
            };

            movie.Reviews.Add(review);

            await _db.SaveChangesAsync();

            return new ReviewDto {
                Id = review.Id,
                Title = movie.Title,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating
            };
        }

        public async Task<IActionResult> DeleteReview(int? id)
        {
            var review = await _db.Reviews.FindAsync(id);
            if (review == null)
            {
                return new NotFoundResult();
            }

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
