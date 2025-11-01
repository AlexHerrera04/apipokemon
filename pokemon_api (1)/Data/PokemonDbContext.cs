using Microsoft.EntityFrameworkCore;
using PokemonAPI.Models;

namespace PokemonAPI.Data
{
    public class PokemonDbContext : DbContext
    {
        public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonType> PokemonTypes { get; set; }
        public DbSet<Stats> Stats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-to-1 between Pokemon and Stats
            modelBuilder.Entity<Pokemon>()
                .HasOne(p => p.Stats)
                .WithOne(s => s.Pokemon)
                .HasForeignKey<Stats>(s => s.PokemonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure 1-to-many for types
            modelBuilder.Entity<Pokemon>()
                .HasMany(p => p.Types)
                .WithOne(t => t.Pokemon)
                .HasForeignKey(t => t.PokemonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
