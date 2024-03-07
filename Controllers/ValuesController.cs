using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Speisekarte.Data;
using Speisekarte.Models;

namespace Speisekarte.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Fill")]
        public void FillData()
        {
            Speise s1 = new Speise { Titel = "Schweinsbraten", Notizen = "Sooooo guad!", Sterne = 4 };
            Zutat z1 = new Zutat { Beschreibung = "Schweinas", Einheit = "g", Menge = 1000 };
            Zutat z2 = new Zutat { Beschreibung = "Sauce", Einheit = "g", Menge = 100 };
            Zutat z3 = new Zutat { Beschreibung = "Semeelknödel", Einheit = "g", Menge = 1000 };

            s1.Zutaten.Add(z1);
            s1.Zutaten.Add(z2);
            s1.Zutaten.Add(z3);

            _context.Speisen.Add(s1);
            _context.SaveChanges();
        }


        [HttpGet]
        [Route("GetAll")]
        public List<Speise> GetSpeisen()
        {
            var speisen = _context.Speisen
                .Include(speise => speise.Zutaten) // Includiert die Zutaten der Klasse
                .ToList();
            return speisen;
        }

        [HttpGet]
        [Route("GetTop")]
        public List<TopSpeiseDto> GetTopSpeisen()
        {
            var topSpeisen = _context.Speisen
                       .OrderByDescending(s => s.Sterne)
                       .Take(3)
                       .Select(s => new TopSpeiseDto
                       {
                           Speise = s.Titel,
                           Sterne = s.Sterne
                       })
                       .ToList();

            return topSpeisen;
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if ( id == null ||_context.Speisen == null)
            {
                return NotFound();
            }

            var speise = await _context.Speisen
                .Include(speise => speise.Zutaten)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (speise == null)
            {
                return NotFound();
            }
            else
            {

                //var zutaten = await _context.Zutaten.Where(z => z.SpeiseId == id).ToListAsync();
                // Alle Zutaten der Speise löschen
                foreach (var zutat in speise.Zutaten)
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
