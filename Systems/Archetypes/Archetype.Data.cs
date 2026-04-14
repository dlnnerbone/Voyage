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

    internal int Increment(int entityID)
    {
        _entityMap.Push(entityID);
        int indexValue = _entityPosition++;

        if (indexValue > Capacity - 1) throw new ArgumentOutOfRangeException($"maximum amount of entities in an archetype has been reached.");
        return indexValue;
    }

    public void ResizeModule<T>(int newLength)
    {
        ushort compID = ComponentMetadata<T>.ID;
        var modID = _indexMap[compID];
        Module<T> module = (Module<T>)this[modID];
        module.ResizeModule(newLength);
    }

    public void ResizeModuleUnsafe<T>(int newLength)
    {
        ushort compID = ComponentMetadata<T>.ID;
        var modID = _indexMap[compID];
        Module<T> module = Unsafe.As<Module<T>>(this[modID]);
        module.ResizeModule(newLength);
    }
}