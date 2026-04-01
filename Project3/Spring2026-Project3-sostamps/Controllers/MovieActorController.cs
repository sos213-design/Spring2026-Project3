using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spring2026_Project3_sostamps.Data;
using Spring2026_Project3_sostamps.Models;
namespace Spring2026_Project3_sostamps.Controllers;
public class MovieActorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public MovieActorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var relationships = _context.MovieActors
            .Include(ma => ma.Movie)
            .Include(ma => ma.Actor)
            .ToList();

        return View(relationships);
    }

    public IActionResult Add()
    {
        ViewBag.Movies = _context.Movies.ToList();
        ViewBag.Actors = _context.Actors.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Add(int movieId, int actorId)
    {
        bool exists = _context.MovieActors
            .Any(ma => ma.MovieId == movieId && ma.ActorId == actorId);

        if (!exists)
        {
            _context.MovieActors.Add(new MovieActor
            {
                MovieId = movieId,
                ActorId = actorId
            });

            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int movieId, int actorId)
    {
        var relation = _context.MovieActors
            .FirstOrDefault(ma => ma.MovieId == movieId && ma.ActorId == actorId);

        if (relation != null)
        {
            _context.MovieActors.Remove(relation);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}