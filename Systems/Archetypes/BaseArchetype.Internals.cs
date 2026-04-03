using System.Collections.Immutable;
using System.Runtime.CompilerServices;
namespace Voyage.Operation;

/// <summary>
/// the Archetype class for storing components. initialization requires an archetype builder or null.
/// </summary>
public partial class Archetype
{
      internal HashSet<Type> _typeSet;
      internal string _collectedTypes;
      internal object[] _dataMatrix;
      internal byte[] _indexMap;
      internal FastStack<int> _entityMap;
      
      public ImmutableHashSet<Type> TypeSet => _typeSet.ToImmutableHashSet();
      public int ArchetypeID { get; internal set; }
      public int TypeCount { get; internal set; }

      public override string ToString() => _collectedTypes ?? string.Empty;
      public static Archetype Null => new();

      internal Archetype()
      {
            _collectedTypes = null!;
            _dataMatrix = null!;
            _indexMap = null!;
            _typeSet = null!;
            ArchetypeID = -1;
            TypeCount = 0;
      }

      public static Archetype Construct<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder => builder.Return();

      public object this[int id] => _dataMatrix[id];
      public ImmutableArray<byte> IndexMap => _indexMap.ToImmutableArray();

      public Module<T> GetModule<T>()
      {
            uint compID = ComponentMetadata<T>.ID;

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
            uint compID = ComponentMetadata<T>.ID;

            byte indexToMod = _indexMap[compID];
            return Unsafe.As<Module<T>>(this[indexToMod]);
      }

      public override int GetHashCode() => HashCode.Combine(_typeSet, ArchetypeID, _collectedTypes, _indexMap, TypeCount);

}