namespace YolkStudio.Pokemon.Core.Trainers;

public record Trainer
{
    public Trainer(int? id,
        string name,
        string region,
        DateTime birthDate,
        DateTime createdAt,
        int wins,
        int losses)
    {
        Id = id;
        Name = name;
        Region = region;
        BirthDate = birthDate;
        CreatedAt = createdAt;
        Wins = wins;
        Losses = losses;
    }

    public int? Id { get; init; }
    public string Name { get; init; }
    public string Region { get; init; }
    public DateTime BirthDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public int Wins { get; init; }
    public int Losses { get; init; }
    public IEnumerable<Pokemons.Pokemon> Pokemons { get; init; } = new List<Pokemons.Pokemon>();
}

