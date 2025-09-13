namespace YolkStudio.Pokemon.Core.Trainers;

public interface ITrainerRepository
{
    Task<Trainer> AddAsync(Trainer trainer);
    Task<bool> DoesExistAsync(string name);
}