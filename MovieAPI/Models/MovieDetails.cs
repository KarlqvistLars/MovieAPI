namespace MovieAPI.Models
{
    public class MovieDetails
    {
        public int Id { get; set; }
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Budget { get; set; } = string.Empty;
    }
}
