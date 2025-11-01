using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonAPI.Models
{
    public class Stats
    {
        [Key, ForeignKey("Pokemon")]
        public int PokemonId { get; set; }

        public int Hp { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }

        public Pokemon? Pokemon { get; set; }
    }
}
