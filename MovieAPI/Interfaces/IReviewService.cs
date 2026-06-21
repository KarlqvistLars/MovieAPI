using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;

namespace MovieAPI.Interfaces
{
    public interface IReviewService
    {
        public Task<ICollection<ReviewDto>> GetReviews();

        public Task<ICollection<ReviewDto>> GetReview(int id);

        public Task<bool> PutReview(int? id, UpdateReviewDto review);

        public Task<ReviewDto?> PostReview(int movieId, ReviewDto reviewDto);

        public Task<IActionResult> DeleteReview(int? id);

    }
}
