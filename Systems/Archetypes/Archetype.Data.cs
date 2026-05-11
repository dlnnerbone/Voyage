using System.Runtime.CompilerServices;
using Voyage.Helper;
namespace Voyage.Operation;

public partial class Archetype : IHasID<int>
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

    public int GetID() => ArchetypeID;
    object IHasID.GetID() => GetID();

    // entity indicing

    internal ushort MoveNext()
    {
        if (Capacity == 0) ResizeModules(8);
        else if (_entityPosition > Capacity - 1) ResizeModules(Capacity * 2);

        return _entityPosition++;
    }

    public void ResizeModules(int newLength)
    {
        Capacity = newLength;
        _archetypeResizer(newLength);
        ArrayHelper<int>.CopyAndResize(ref _entityMap, Capacity);
    }

    public Entity GetEntity(World world, int entityMapIndex) => world._entities[_entityMap[entityMapIndex]];
}