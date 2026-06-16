namespace MovieAPI.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;
        // M:M till Movies genom MovieActor
        public IEnumerable<Movie>? Movies { get; set; }
    }
}
