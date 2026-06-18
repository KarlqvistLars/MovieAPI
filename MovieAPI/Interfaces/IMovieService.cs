using Microsoft.AspNetCore.Mvc;
using MovieAPI.DTOs;

namespace MovieAPI.Interfaces
{
    public interface IMovieService
    {
        public Task<ActionResult<ICollection<MovieDto>>> GetMovies();
        public Task<MovieDto?> GetMovieById(int id);

    }
}
