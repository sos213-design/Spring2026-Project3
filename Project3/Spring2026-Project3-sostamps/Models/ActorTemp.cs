namespace Spring2026_Project3_sostamps.Models;

public class ActorTemp
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Gender { get; set; }
    public required int Age { get; set; }
    public required string IMDBLink { get; set; }
    public required byte[] Portrait { get; set; }
    public required string Movies { get; set; }
}