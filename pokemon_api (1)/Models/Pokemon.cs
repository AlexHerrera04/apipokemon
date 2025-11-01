using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PokemonAPI.Models
{
    public class Pokemon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Generation { get; set; }
        public bool Legendary { get; set; }

        public ICollection<PokemonType> Types { get; set; } = new List<PokemonType>();
        public Stats? Stats { get; set; }
    }
}
