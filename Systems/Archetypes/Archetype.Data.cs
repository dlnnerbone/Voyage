using System.Runtime.CompilerServices;
namespace Voyage.Operation;

public partial class Archetype : IHasID<ushort>
{
    public ref TComp GetComponent<TComp>(int columnIndex)
    {
        ushort compID = ComponentMetadata<TComp>.ID;
        byte rowIndex = _indexMap[compID];
        var module = (Module<TComp>)_dataMatrix[rowIndex];

        return ref module[columnIndex];
    }

    public ref TComp GetComponentUnsafe<TComp>(int columnIndex)
    {
        ushort compID = ComponentMetadata<TComp>.ID;
        byte rowIndex = _indexMap[compID];
        var module = Unsafe.As<Module<TComp>>(this[rowIndex]);

        return ref Unsafe.AsRef(ref module[columnIndex]);
    }

    public ushort GetID() => ArchetypeID;
    object IHasID.GetID() => GetID();

    // entity indicing

    public void Increment(ref Entity entity)
    {
        var index = _entityPosition++;
        
        if (entity.IsNull()) throw new ArgumentException($"entity is 'null' and cannot be incremented.");
        else if (index > Capacity - 1) ResizeModules(Capacity * 2);

        _entityMap[index] = entity.EntityID;
        entity = new Entity(entity.EntityID, ArchetypeID, index);
    }

    public void ResizeModules(int newLength)
    {
        Capacity = newLength;
        _archetypeResizer(newLength);
    }

    public Entity GetEntity(World world, int entityMapIndex) => world._entities[_entityMap[entityMapIndex]];
}