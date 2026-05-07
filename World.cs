using Voyage.Operation;
namespace Voyage;

public class World
{
    internal readonly Query _query;
    internal readonly FastStack<Entity> _entities;

    public Entity this[int index] => _entities[index];
    
    public Archetype GetArchetype(Entity entity) => _query[entity.ArchetypeID];
    public Archetype GetArchetype(Entity.Reader reader) => _query[reader.ArchetypeID];
    public Archetype GetArchetype(int archetypeID) => _query[archetypeID];

    public World()
    {
        if (!PrimaryWorld.IsNull()) throw new ArgumentException($"can not have more than one world initialized.");
        _query = new(this);
        _entities = FastStack<Entity>.Create(4);
        PrimaryWorld._world = this;
    }

    public World(int entityInitLength)
    {
        if (!PrimaryWorld.IsNull()) throw new ArgumentException($"can not have more than one world initialized.");
        _query = new(this);
        _entities = FastStack<Entity>.Create(entityInitLength);
        PrimaryWorld._world = this;
    }

    public Entity.Reader ReadEntity(int index) => _entities[index].Read();

    public Archetype ConstructArchetype<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder => _query.ConstructArchetype(builder); 
    public Archetype ConstructArchetype(ArchetypeBuilder builder) => _query.ConstructArchetype(builder);

    public int CreateEntity()
    {
        var index = _entities.Push(Entity.Null);
        _entities[index] = new Entity(index, 0, 0);
        return index;
    }

    public int CreateEntity(Archetype arch)
    {
        if (arch.IsNull() || arch == null) throw new ArgumentException($"Archetype can not be 'null'");
        var index = _entities.Push(Entity.Null);
        arch.AddEntity(ref _entities[index]);
        return index;
    }

    public ref TComp GetComponent<TComp>(int entityID)
    {
        var entity = this[entityID];
        var arch = _query[entity.ArchetypeID];
        return ref arch.GetComponent<TComp>(entity.Queue);
    }
}