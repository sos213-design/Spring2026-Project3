namespace Spring2026_Project3_sostamps.Models;

public class MovieActor
{
    public int MovieId { get; set; }
    public MovieTemp Movie { get; set; }
    
    public int ActorId { get; set; }
    public ActorTemp Actor { get; set; }
}