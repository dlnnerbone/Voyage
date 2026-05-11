using System.Collections.Immutable;
using System.Runtime.CompilerServices;
namespace Voyage.Operation;

/// <summary>
/// the Archetype class for storing components. initialization requires an archetype builder or null.
/// </summary>
public partial class Archetype : INull
{
      internal HashSet<Type> _typeSet;
      internal string _collectedTypes;
      internal object[] _dataMatrix;
      internal byte[] _indexMap; // index map for components
      internal int[] _entityMap; // index map for entities from worlds
      
      public ImmutableHashSet<Type> TypeSet => _typeSet.ToImmutableHashSet();
      public ushort ArchetypeID { get; internal set; }
      public int TypeCount { get; internal set; }
      internal ushort _entityPosition = 0;
      public int Capacity { get; internal set; } = 0;
      internal Action<int> _archetypeResizer;

      public override string ToString() => _collectedTypes ?? string.Empty;
      public static Archetype Null => new();
      public bool IsNull() => ArchetypeID == 0;

      internal Archetype()
      {
            _collectedTypes = null!;
            _entityMap = null!;
            _dataMatrix = null!;
            _indexMap = null!;
            _typeSet = null!;
            ArchetypeID = 0;
            TypeCount = 0;
            _archetypeResizer = null!;
      }

      public static Archetype Construct<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder => builder.Return();
      public static Archetype Construct(ArchetypeBuilder builder) => builder.Return();

      public object this[int id] => _dataMatrix[id];
      public ImmutableArray<byte> IndexMap => _indexMap.ToImmutableArray();

      public Module<T> GetModule<T>()
      {
            ushort compID = ComponentMetadata<T>.ID;

            if (compID > _indexMap.Length - 1)
            {
                  throw new ArgumentOutOfRangeException($"{typeof(T)} is not in the archetype. {compID} > {_indexMap.Length - 1}");
            }

            byte indexToMod = _indexMap[compID];

            return (Module<T>)_dataMatrix[indexToMod];
      }

      // note: this method is Unsafe.
      public Module<T> UnboxModule<T>()
      {
            ushort compID = ComponentMetadata<T>.ID;

            byte indexToMod = _indexMap[compID];
            return Unsafe.As<Module<T>>(this[indexToMod]);
      }

      public override int GetHashCode() => HashCode.Combine(_typeSet, ArchetypeID, _collectedTypes, _indexMap, TypeCount, Capacity, _archetypeResizer);

}