using Microsoft.AspNetCore.Mvc;
using Spring2026_Project3_sostamps.Models.DetailModels;
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
}