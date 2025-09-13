using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Infrastructure.Data;

public class PokemonDbContext : DbContext
{
    public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options)
    {
        
    }

    public DbSet<Trainer> Trainers => Set<Trainer>();
}