using Voyage.Operation;
using Voyage.Helper;
namespace Voyage;

public sealed class World
{
    internal Query _query;
    internal Entity[] _entities;
    internal int _indexPosition = 0;

    public int EntityCount => _entities.Length;

    public ref readonly Entity this[int index] => ref _entities[index];
    
    public Archetype GetArchetype(Entity entity) => _query[entity.ArchetypeID];
    public Archetype GetArchetype(Entity.Reader reader) => _query[reader.ArchetypeID];
    public Archetype GetArchetype(int archetypeID) => _query[archetypeID];

    public Entity.Reader ReadEntity(int index) => _entities[index].Read();

    public Archetype ConstructArchetype<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder => _query.ConstructArchetype(builder); 
    public Archetype ConstructArchetype(ArchetypeBuilder builder) => _query.ConstructArchetype(builder);

    public World()
    {
        _query = new(this);
        _entities = new Entity[16];
        _indexPosition = 0;
    }

    public World(int initEntityArrLength)
    {
        _query = new(this);
        _entities = new Entity[initEntityArrLength];
        _indexPosition = 0;
    }

    public int CreateNullEntity()
    {
        if (EntityCount == 0) Array.Resize(ref _entities, 16);
        var nextIndex = _indexPosition++;
        if (nextIndex > EntityCount - 1) ArrayHelper<Entity>.CopyAndResize(ref _entities, EntityCount * 2);
        
        _entities[nextIndex] = Entity.Null;
        return nextIndex;
    }

    /// <summary>
    /// Creats an entity not occupied by any archetype and returns the index value that maps to the entity in the world.
    /// </summary>
    /// <returns>returns the zero-based index value to the entity reader that was created within the world.</returns>
    public Entity.Reader CreateEntity()
    {
        var nextIndex = _indexPosition++;
        if (EntityCount == 0) Array.Resize(ref _entities, 16);
        else if (nextIndex > EntityCount - 1) ArrayHelper<Entity>.CopyAndResize(ref _entities, EntityCount * 2);

        _entities[nextIndex] = new Entity(nextIndex, 0, 0);
        return _entities[nextIndex];
    }

    public Entity.Reader CreateEntity(Archetype archetype)
    {
        var nextIndex = _indexPosition++;

        if (EntityCount == 0) Array.Resize(ref _entities, 16);
        else if (nextIndex > EntityCount - 1) ArrayHelper<Entity>.CopyAndResize(ref _entities, EntityCount * 2);

        if (archetype.IsNull()) throw new ArgumentException("archetype can not be null.");
        _entities[nextIndex] = new Entity(nextIndex, archetype.ArchetypeID, archetype.MoveNext());
        return _entities[nextIndex];
    }
}