using Microsoft.AspNetCore.Mvc;
using Spring2026_Project3_sostamps.Models;
using Spring2026_Project3_sostamps.Data;

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
    
}