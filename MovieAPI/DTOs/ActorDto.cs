namespace MovieAPI.DTOs
{
    public class ActorDto
    {
        public int ActorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;
        public List<string> Movies { get; set; } = new List<string>();
        //public ICollection<MovieDto>? Movies { get; set; } = new List<MovieDto>();
    }
}
