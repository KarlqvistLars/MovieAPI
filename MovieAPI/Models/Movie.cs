namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public IEnumerable<Genre>? Genres { get; set; }
        public MovieDetails? Details { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Actor>? Actors { get; set; }
    }
}
