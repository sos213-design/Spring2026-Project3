using Microsoft.AspNetCore.Mvc;
using Spring2026_Project3_sostamps.Models.DetailModels;
using Spring2026_Project3_sostamps.Models;
using Spring2026_Project3_sostamps.Data;
using VaderSharp2;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace Spring2026_Project3_sostamps.Controllers;

public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    
    public MoviesController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    //View
    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();
        return View(movies);
    }

    // Add
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(MovieTemp movie, IFormFile posterFile)
    {
        if (posterFile != null)
        {
            using (var ms = new MemoryStream())
            {
                await posterFile.CopyToAsync(ms);
                movie.Poster = ms.ToArray();
            }
        }
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }

    
    // Delete
    public IActionResult Delete(int id)
    {
        var movies = _context.Movies.ToList();
        return View(movies);
    }

    [HttpPost]
    public IActionResult DeleteSelected(List<int> selectedIds)
    {
        if (selectedIds != null && selectedIds.Count > 0)
        {
            var moviesToDelete = _context.Movies
                .Where(m => selectedIds.Contains(m.Id))
                .ToList();
            
            _context.Movies.RemoveRange(moviesToDelete);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // Reviews & Details
    public async Task<IActionResult> Details(int id)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }
        
        // AI Reviews
        var apiKey = _configuration["OpenAI:ApiKey"];
        var endpoint = _configuration["OpenAI:Endpoint"];

        var azureClient = new AzureOpenAIClient(
            new Uri(endpoint),
            new System.ClientModel.ApiKeyCredential(apiKey)
        );

        var chatClient = azureClient.GetChatClient("gpt-4.1-mini");

        var response = await chatClient.CompleteChatAsync(new[]
        {
            new UserChatMessage(
                $"Generate 5 reviews for movie {movie.Title}. Make some serious and some funny. Make them no more than 3 sentences.")
        });

        string result = response.Value.Content[0].Text;

        var reviews = result.Split('\n')
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .Take(5)
            .ToList();
        
        // Positivity Analyzer
        var analyzer = new SentimentIntensityAnalyzer();
        
        var reviewData = reviews.Select(r => new Review()
        {
            Text = r,
            Rating = analyzer.PolarityScores(r).Compound
        }).ToList();

        double avgReview = reviewData.Average(r => r.Rating);

        var vm = new Details()
        {
            Movie = movie,
            Reviews = reviewData,
            Rating = avgReview
        };

        return View(vm);
    }
}