namespace MovieAPI.DTOs
{
    public class ReviewDto
    {
        // Skicka med film titel för identifiering i frontend
        public string? Title { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; } // Rating (1–5)
    }
}
