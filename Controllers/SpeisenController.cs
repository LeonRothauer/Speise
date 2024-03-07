using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Speisekarte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeisenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpeisenController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Fill")]
        public void FillData()
        {
            Speise s1 = new Speise { Titel ="Nachspeise", Notizen="Meine Notizen dazu", Sterne=3};
            Zutat z1 = new Zutat { Beschreibung = "Huhn", Einheit = "g", Menge = 200 };
            Zutat z2 = new Zutat { Beschreibung = "Gewürze", Einheit = "g", Menge = 50 };
            Zutat z3 = new Zutat { Beschreibung = "Wasser", Einheit = "ml", Menge = 100 };

            s1.Zutaten.Add(z1);
            s1.Zutaten.Add(z2);
            s1.Zutaten.Add(z3);

            _context.Speisen.Add(s1);
            _context.SaveChanges();


        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetSpeisen()
        {
            var speisen = _context.Speisen
                .Include(speise => speise.Zutaten)
                .ToList();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            var json = JsonSerializer.Serialize(speisen, options);

            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.Speisen == null)
            {
                return NotFound();
            }

            var speise = await _context.Speisen
                .Include(speise => speise.Zutaten)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(speise == null)
            {
                return NotFound();
            }
            {
                foreach(var zutat in speise.Zutaten)
                {
                    _context.Zutaten.Remove(zutat);
                }
            }
            _context.Speisen.Remove(speise);

            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
