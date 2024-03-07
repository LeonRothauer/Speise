using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;
using System.Diagnostics;

namespace Speisekarte.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var speisen = _context.Speisen.Include(s => s.Zutaten).ToList();
            return View(speisen);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
 
    }
}
