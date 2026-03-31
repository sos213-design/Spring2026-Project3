using Microsoft.AspNetCore.Mvc;
using Spring2026_Project3_sostamps.Models.DetailModels;
using Spring2026_Project3_sostamps.Models;
using Spring2026_Project3_sostamps.Data;
using VaderSharp2;

namespace Spring2026_Project3_sostamps.Controllers;

public class MoviesController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        var movies = _context.Movies.ToList();
        return View(movies);
    }
    //Add
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
//Delete
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

    public async Task<IActionResult> Details(int id)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

        if (movie == null)
        {
            return NotFound();
        }

        //Todo: Call AI need to set up
        List<string> reviews = new List<string>
        {
            "Amazing movie with great acting.",
            "A bit confusing but very creative.",
            "Fantastic visuals and storytelling.",
            "Too complex for casual viewers.",
            "One of the best movies ever made."
        };
        
        //Positivity Analyzer
        var analyzer = new SentimentIntensityAnalyzer();
        
        var reviewData  = reviews.Select(r => new Review()
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