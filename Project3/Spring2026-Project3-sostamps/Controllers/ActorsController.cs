using Microsoft.AspNetCore.Mvc;
using Spring2026_Project3_sostamps.Models.ActDetailModel;
using Spring2026_Project3_sostamps.Models;
using Spring2026_Project3_sostamps.Data;
using VaderSharp2;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace Spring2026_Project3_sostamps.Controllers;

public class ActorsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    
    public ActorsController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var actors = _context.Actors.ToList();
            return View(actors);
    }
    
    //Add
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(ActorTemp actor)
    {
        // var newActor = new Actor
        // {
        //     Name = actor.Name,
        // }
            
            
        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    //Delete
    public IActionResult Delete(int id)
    {
        var actors = _context.Actors.ToList();
        return View(actors);
    }

    [HttpPost]
    public IActionResult DeleteSelected(List<int> selectedIds)
    {
        if (selectedIds != null && selectedIds.Count > 0)
        {
            var actorstoDelete =  _context.Actors
                .Where(a => selectedIds.Contains(a.Id))
                .ToList();
            
            _context.Actors.RemoveRange(actorstoDelete);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var actor = _context.Actors.FirstOrDefault(a => a.Id == id);

        if (actor == null)
        {
            return NotFound();
        }

        // Azure AI setup (SAME as movie)
        var apiKey = _configuration["OpenAI:ApiKey"];
        var endpoint = _configuration["OpenAI:Endpoint"];

        var azureClient = new AzureOpenAIClient(
            new Uri(endpoint),
            new System.ClientModel.ApiKeyCredential(apiKey)
        );

        // This MUST match your deployment name
        var chatClient = azureClient.GetChatClient("gpt-4.1-mini");

        // AI Tweets
        var response = await chatClient.CompleteChatAsync(new[]
        {
            new UserChatMessage(
                $"Give exactly 10 short tweets about actor '{actor.Name}'. Each tweet must be on its own line with no numbering or bullets.")
        });

        string result = response.Value.Content[0].Text;

        var tweets = result.Split('\n')
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .Take(10)
            .ToList();

        // Sentiment
        var analyzer = new SentimentIntensityAnalyzer();

        var tweetData = tweets.Select(t => new Twitter()
        {
            Tweet = t,
            Rating = analyzer.PolarityScores(t).Compound
        }).ToList();

        double avgRating = tweetData.Average(t => t.Rating);

        var vm = new Details()
        {
            Actor = actor,
            Tweets = tweetData,
            Rating = avgRating
        };

        return View(vm);
    }
    
}