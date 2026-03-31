using System.Collections.Immutable;
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
      
      public ImmutableHashSet<Type> TypeSet => _typeSet.ToImmutableHashSet();
      public int ArchetypeID { get; internal set; }
      public int TypeCount { get; internal set; }
      
      private int _entityPosition = 0;

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

      public static Archetype Construct<TBuilder>(TBuilder builder) where TBuilder : IArchetypeBuilder
      {
            return builder.Return();
      }

      public object this[int id] => _dataMatrix[id];

      public ImmutableArray<byte> IndexMap => _indexMap.ToImmutableArray();

      /// <summary>
      /// this method returns the desired module.
      /// 
      /// note: this method assumes the <see cref="_dataMatrix"> array  has Module elements.
      /// </summary>
      /// <typeparam name="T">the type to look for.</typeparam>
      /// <returns>returns the module holding the specified type.</returns>
      /// <exception cref="ArgumentOutOfRangeException">this exception is called when the Module types ID is not in the bounds of the array.</exception>
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

}