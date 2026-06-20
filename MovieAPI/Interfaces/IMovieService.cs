using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;

namespace MovieAPI.Interfaces
{
    public interface IMovieService
    {
        public Task<ICollection<MovieDto>?> GetMovies();
        public Task<MovieDto?> GetMovieById(int id);
        public Task<MovieDto?> GetMovieReviews(int id);
        public Task<MovieDto?> GetMovieDetails(int id);
        public Task<IActionResult> PutMovie(int id, MovieDto movieDto);
        Task<ActionResult<MovieDto>> PostMovie(MovieDto movieDto);
        public Task<IActionResult> DeleteMovie(int? id);
    }
}
