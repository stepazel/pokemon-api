using YolkStudio.Pokemon.Core.Trainers;

namespace YolkStudio.Pokemon.Core.Pokemons;

public record Pokemon
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int Level { get; init; }
    public int Health { get; init; }
    
    public int? OwnerId { get; init; } 
    public Trainer? Owner { get; init; } 
    
    public int TypeId { get; init; }
    public Element Type { get; init; }
    
    public DateTime? CaughtAt { get; init; }
    
    public Pokemon(int id, string name, int level, int health, DateTime? caughtAt, int? ownerId, int typeId)
    {
        Id = id;
        Name = name;
        Level = level;
        Health = health;
        CaughtAt = caughtAt;
        OwnerId = ownerId;
        TypeId = typeId;
    }
}


public record Element
{
    public int Id { get; init; }
    public string Name { get; init; }

    public ICollection<Pokemon> Pokemons { get; init; } = new List<Pokemon>();
}

