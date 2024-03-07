using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json.Serialization;

namespace Speisekarte.Models
{
    public class Speise
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string? Notizen { get; set; }
        public int Sterne { get; set; }
        
        // 1:n Beziehung
        [JsonIgnore]
        public List<Zutat> Zutaten { get; set; } = new List<Zutat>();



    }
}
