namespace MovieAPI.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Duration { get; set; }
        public MovieDetailsDto Details { get; set; } = new MovieDetailsDto();
        public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
        public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
