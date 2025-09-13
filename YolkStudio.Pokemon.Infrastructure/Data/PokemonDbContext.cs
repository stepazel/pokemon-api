using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Trainers;
using YolkStudio.Pokemon.Core.Pokemons;

namespace YolkStudio.Pokemon.Infrastructure.Data;

public class PokemonDbContext : DbContext
{
    public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options)
    {
    }

    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<Core.Pokemons.Pokemon> Pokemons => Set<Core.Pokemons.Pokemon>();
    public DbSet<Element> Elements => Set<Element>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Pokemons.Pokemon>(pokemon =>
        {
            pokemon.HasKey(p => p.Id);

            pokemon.HasOne(p => p.Owner)
                .WithMany(t => t.Pokemons)
                .HasForeignKey(p => p.OwnerId);
                
            pokemon.HasOne(p => p.Type)
                .WithMany(e => e.Pokemons)
                .HasForeignKey(p => p.TypeId)
                .IsRequired();
        });
    }

}