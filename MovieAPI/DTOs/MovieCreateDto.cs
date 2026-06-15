using MovieAPI.Models;

namespace MovieAPI.DTOs
{
    public class MovieCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        // Genres
        public IEnumerable<Genre>? Genres { get; set; }
        // MovieDetails
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Budget { get; set; } = string.Empty;
        // Reviews
        public IEnumerable<Review>? Reviews { get; set; }
        // Actors
        public IEnumerable<Actor>? Actors { get; set; }
    }
}
