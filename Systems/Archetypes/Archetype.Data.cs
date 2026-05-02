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

    internal Entity Add(int id)
    {
        if (IsNull()) throw new ArgumentException("can not increment an entity into an archetype that's null."); 
        else if (Capacity == 0) ResizeModules(4);
        ushort queue = _entityPosition++;
        if (queue > Capacity - 1) ResizeModules(Capacity * 2);
        _entityMap[queue] = id;
        return new Entity(id, (ushort)ArchetypeID, queue);
    }

    // entity indicing

    public void ResizeModules(int newLength)
    {
        Capacity = newLength;
        _archetypeResizer(newLength);
        ArrayHelper<int>.CopyAndResize(ref _entityMap, Capacity);
    }

    public Entity GetEntity(World world, int entityMapIndex) => world._entities[_entityMap[entityMapIndex]];
}