namespace Spring2026_Project3_sostamps.Models;

public class MovieTemp
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public int Year { get; set; }
    public required string IMDBLink { get; set; }
    public required byte[] Poster { get; set; }
    public string Actors { get; set; }
}