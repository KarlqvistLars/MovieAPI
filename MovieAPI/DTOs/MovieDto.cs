namespace MovieAPI.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Duration { get; set; }
        public MovieDetailsDto Details { get; set; }
        public List<ActorDto> Actors { get; set; }
        public List<GenreDto> Genres { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
