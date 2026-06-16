namespace MovieAPI.Models
{
    public class Temp_MovieActor
    {
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }

        // Primärnyckel (composite key)    
        public int Id { get; set; } // Alternativ: bara MovieId + ActorId som PK
    }
}
