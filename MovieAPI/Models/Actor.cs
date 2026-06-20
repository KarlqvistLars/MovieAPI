namespace MovieAPI.Models
{
    public class Actor
    {
        public int ActorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;
        // M:M till Movies genom MovieActor
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();

    }
}
