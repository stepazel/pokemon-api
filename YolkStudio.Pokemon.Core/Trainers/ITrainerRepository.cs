namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerRepository
{
    Task<Trainer> AddAsync(Trainer trainer);
    Task<Trainer?> GetAsync(int id);
    Task<bool> DoesExistAsync(string name);
    Task<IEnumerable<Trainer>> GetAllAsync(); 
    Task<Trainer?> GetTrainerWithPokemonsAsync(int id);
    Task SaveChangesAsync();
}