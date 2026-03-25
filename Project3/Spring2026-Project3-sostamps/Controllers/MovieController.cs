using Microsoft.AspNetCore.Mvc;

namespace Spring2026_Project3_sostamps.Controllers;

public class MovieController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}