namespace MovieAPI.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;

        // M:M till Movies genom MovieGenre
        public IEnumerable<Movie>? Movies { get; set; }
    }
}
