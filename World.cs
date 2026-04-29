using Voyage.Operation;
namespace Voyage;

public class World : IHasID<int>
{
    internal readonly Query _query;
    internal readonly FastStack<Entity> _entities;
    
    public readonly int WorldID;

    public Entity this[int index] => _entities[index];
    
    public Archetype GetArchetype(Entity entity) => _query[entity.ArchetypeID];
    public Archetype GetArchetype(Entity.EntityLookup lookup) => _query[lookup.ArchetypeID];
    public Archetype GetArchetype(Entity.EntityReader reader) => _query[reader.ArchetypeID];
    public Archetype GetArchetype(int archetypeID) => _query[archetypeID];

    public int GetID() => WorldID;
    object IHasID.GetID() => GetID();

    public World(int worldID)
    {
        WorldID = worldID;
        _query = new(this);
        _entities = FastStack<Entity>.Create(4);
    }

    public World(int worldID, int entityInitLength)
    {
        WorldID = worldID;
        _query = new(this);
        _entities = FastStack<Entity>.Create(entityInitLength);
    }

    public Entity.EntityReader ReadEntity(int index) => _entities[index].Read();
    public Entity.EntityLookup LookupEntity(int index) => _entities[index].GetEntityLookup();

    public Archetype ConstructArchetype<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder => _query.ConstructArchetype(builder); 
    public Archetype ConstructArchetype(ArchetypeBuilder builder) => _query.ConstructArchetype(builder);

}