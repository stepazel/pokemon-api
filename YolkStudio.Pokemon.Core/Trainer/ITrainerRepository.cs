namespace YolkStudio.Pokemon.Core.Trainer;

public interface ITrainerRepository
{
    Task<Trainer> AddAsync(Trainer trainer);
    Task<bool> DoesExistAsync(string name);
}