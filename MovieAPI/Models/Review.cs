namespace MovieAPI.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } // Rating (1–5)
        // 1:M till Movie
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
