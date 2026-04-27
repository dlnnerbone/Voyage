using Voyage.Operation;
namespace Voyage.Marshals;

public static class ArchetypeMaster
{
    public static ref HashSet<Type> AttainHashSet(Archetype archetype) => ref archetype._typeSet;
    public static ref string AttainTypeString(Archetype archetype) => ref archetype._collectedTypes;
    public static ref object[] AttainModules(Archetype archetype) => ref archetype._dataMatrix;
    public static ref byte[] AttainIndexMap(Archetype archetype) => ref archetype._indexMap;
    public static ref int[] AttainEntityMap(Archetype archetype) => ref archetype._entityMap;

    public static ref ushort AttainPosition(Archetype archetype) => ref archetype._entityPosition;
    public static ref Action<int> AttainResizer(Archetype archetype) => ref archetype._archetypeResizer;
}