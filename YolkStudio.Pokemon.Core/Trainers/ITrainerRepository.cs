namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerRepository
{
    Task<Trainer> AddAsync(Trainer trainer);
    Task<Trainer?> GetByIdAsync(int id);
    Task<bool> DoesExistAsync(string name);
    Task<bool> DoesExistAsync(int id);
    Task<IEnumerable<Trainer>> GetAllAsync(); 
}