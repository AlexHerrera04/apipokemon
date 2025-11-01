using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonAPI.Data;
using PokemonAPI.Models;
using PokemonAPI.Services;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonDbContext _context;
        private readonly AuthService _authService;

        public PokemonController(PokemonDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET /pokemon
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pokemons = await _context.Pokemons
                .Include(p => p.Types)
                .Include(p => p.Stats)
                .OrderBy(p => p.Id)
                .ToListAsync();

            return Ok(new { count = pokemons.Count, results = pokemons });
        }

        // GET /pokemon/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pokemon = await _context.Pokemons
                .Include(p => p.Types)
                .Include(p => p.Stats)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pokemon == null) return NotFound();
            return Ok(pokemon);
        }

        // GET /pokemon/filtrar?type_1=&type_2=&name=&legendary=&generation=
        [HttpGet("filtrar")]
        public async Task<IActionResult> Filter(
            string? name,
            string? type_1,
            string? type_2,
            bool? legendary,
            int? generation,
            string? statField,
            int? statValue
        )
        {
            var query = _context.Pokemons
                .Include(p => p.Types)
                .Include(p => p.Stats)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (legendary.HasValue)
                query = query.Where(p => p.Legendary == legendary.Value);

            if (generation.HasValue)
                query = query.Where(p => p.Generation == generation.Value);

            if (!string.IsNullOrEmpty(type_1))
                query = query.Where(p => p.Types.Any(t => t.TypeName.ToLower() == type_1.ToLower() && t.Slot == 1));

            if (!string.IsNullOrEmpty(type_2))
                query = query.Where(p => p.Types.Any(t => t.TypeName.ToLower() == type_2.ToLower() && t.Slot == 2));

            // statField: one of total/hp/attack/defense/specialattack/specialdefense/speed
            if (!string.IsNullOrEmpty(statField) && statValue.HasValue)
            {
                switch (statField.ToLower())
                {
                    case "hp": query = query.Where(p => p.Stats != null && p.Stats.Hp >= statValue.Value); break;
                    case "attack": query = query.Where(p => p.Stats != null && p.Stats.Attack >= statValue.Value); break;
                    case "defense": query = query.Where(p => p.Stats != null && p.Stats.Defense >= statValue.Value); break;
                    case "specialattack": query = query.Where(p => p.Stats != null && p.Stats.SpecialAttack >= statValue.Value); break;
                    case "specialdefense": query = query.Where(p => p.Stats != null && p.Stats.SpecialDefense >= statValue.Value); break;
                    case "speed": query = query.Where(p => p.Stats != null && p.Stats.Speed >= statValue.Value); break;
                    case "total": query = query.Where(p => p.Stats != null && (p.Stats.Hp + p.Stats.Attack + p.Stats.Defense + p.Stats.SpecialAttack + p.Stats.SpecialDefense + p.Stats.Speed) >= statValue.Value); break;
                }
            }

            var results = await query.OrderBy(p => p.Id).ToListAsync();

            var filters = new Dictionary<string, object?>();
            if (!string.IsNullOrEmpty(type_1)) filters["type_1"] = type_1;
            if (!string.IsNullOrEmpty(type_2)) filters["type_2"] = type_2;
            if (!string.IsNullOrEmpty(name)) filters["name"] = name;
            if (legendary.HasValue) filters["legendary"] = legendary.Value;
            if (generation.HasValue) filters["generation"] = generation.Value;
            if (!string.IsNullOrEmpty(statField)) filters["stat"] = statField;

            return Ok(new { filters, count = results.Count, results });
        }

        // PUT /pokemon/{id} - protected by API key
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pokemon updated)
        {
            // simple API key check
            if (!_authService.ValidateApiKey(Request))
                return Unauthorized();

            var existing = await _context.Pokemons
                .Include(p => p.Types)
                .Include(p => p.Stats)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existing == null) return NotFound();

            // update simple fields
            existing.Name = updated.Name;
            existing.Generation = updated.Generation;
            existing.Legendary = updated.Legendary;

            // stats
            if (updated.Stats != null)
            {
                if (existing.Stats == null)
                {
                    existing.Stats = updated.Stats;
                    existing.Stats.PokemonId = existing.Id;
                }
                else
                {
                    existing.Stats.Hp = updated.Stats.Hp;
                    existing.Stats.Attack = updated.Stats.Attack;
                    existing.Stats.Defense = updated.Stats.Defense;
                    existing.Stats.SpecialAttack = updated.Stats.SpecialAttack;
                    existing.Stats.SpecialDefense = updated.Stats.SpecialDefense;
                    existing.Stats.Speed = updated.Stats.Speed;
                }
            }

            // types: for simplicity replace existing types with provided ones
            if (updated.Types != null)
            {
                // remove existing
                _context.PokemonTypes.RemoveRange(existing.Types);
                existing.Types.Clear();

                foreach (var t in updated.Types)
                {
                    existing.Types.Add(new PokemonType { TypeName = t.TypeName, Slot = t.Slot, PokemonId = existing.Id });
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /pokemon/{id} - protected by API key
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_authService.ValidateApiKey(Request))
                return Unauthorized();

            var existing = await _context.Pokemons.FindAsync(id);
            if (existing == null) return NotFound();

            _context.Pokemons.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
