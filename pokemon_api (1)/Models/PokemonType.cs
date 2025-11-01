using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonAPI.Models
{
    public class PokemonType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TypeName { get; set; } = string.Empty;

        // Slot: 1 = primary, 2 = secondary
        public int Slot { get; set; }

        [ForeignKey("Pokemon")]
        public int PokemonId { get; set; }
        public Pokemon? Pokemon { get; set; }
    }
}
