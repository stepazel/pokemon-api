using Microsoft.EntityFrameworkCore;
using YolkStudio.Pokemon.Core.Trainer;
using YolkStudio.Pokemon.Infrastructure.Data;

namespace YolkStudio.Pokemon.Infrastructure.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly PokemonDbContext _context;

    public TrainerRepository(PokemonDbContext context)
    {
        _context = context;
    }

    public async Task<Trainer> AddAsync(Trainer trainer)
    {
        _context.Trainers.Add(trainer);
        await _context.SaveChangesAsync();
        return trainer;
    }

    public async Task<bool> DoesExistAsync(string name)
    {
        return await _context.Trainers.AnyAsync(x => x.Name == name);
    }
}