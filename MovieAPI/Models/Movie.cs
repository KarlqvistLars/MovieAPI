namespace MovieAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;

        // 1:1 till Details
        public int? DetailsId { get; set; }
        public MovieDetails? Details { get; set; }
        // M:M till Actors och Genres genom MovieActor och MovieGenre
        public IEnumerable<Actor>? Actors { get; set; }
        public IEnumerable<Genre>? Genres { get; set; }
        // 1:M till Reviews
        public IEnumerable<Review>? Reviews { get; set; }
    }
}
