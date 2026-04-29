using System.Runtime.CompilerServices;
using Voyage.Helper;
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
        
    }

    public void ResizeModules(int newLength)
    {
        Capacity = newLength;
        _archetypeResizer(newLength);
        ArrayHelper<int>.CopyAndResize(ref _entityMap, Capacity);
    }

    public Entity GetEntity(World world, int entityMapIndex) => world._entities[_entityMap[entityMapIndex]];
}