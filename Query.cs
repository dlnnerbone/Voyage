using Voyage.Operation;
namespace Voyage;

public class Query
{
    internal readonly World _world;

    internal readonly FastStack<Archetype> _archetypes;
    internal Archetype this[int index] => _archetypes[index];

    internal Query(World world)
    {
        _world = world;
        _archetypes = FastStack<Archetype>.Create(4);
    }

    internal Query(World world, int initArchetypeCount)
    {
        _world = world;
        _archetypes = FastStack<Archetype>.Create(initArchetypeCount);
    }

    public Archetype ConstructArchetype<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder
    {
        var arch = Archetype.Construct(builder);
        var archID = _archetypes.Push(arch);
        arch.ArchetypeID = archID;
        return arch;
    }

    public Archetype ConstructArchetype(ArchetypeBuilder builder)
    {
        var arch = Archetype.Construct(builder);
        var archID = _archetypes.Push(arch);
        arch.ArchetypeID = archID;
        return arch;
    }
    
}