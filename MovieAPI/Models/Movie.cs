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
        public ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        // 1:M till Reviews
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
