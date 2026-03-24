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
      internal sbyte[] _indexMap;

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

      public object this[int id] => _dataMatrix[id];

      /// <summary>
      /// this method returns the desired module.
      /// 
      /// note: this method assumes the <see cref="_dataMatrix"> array  has Module elements.
      /// </summary>
      /// <typeparam name="T">the type to look for.</typeparam>
      /// <returns>returns the module holding the specified type.</returns>
      /// <exception cref="ArgumentOutOfRangeException">this exception is called when the Module types ID is not in the bounds of the array.</exception>
      /// <exception cref="InvalidCastException">if compID is -1, component is not within the bounds of the array.</exception>
      public Module<T> GetModule<T>()
      {
            uint compID = ComponentMetadata<T>.ID;

            if (compID > _indexMap.Length - 1)
            {
                  throw new ArgumentOutOfRangeException($"{typeof(T)} is not in the archetype. {compID} > {_indexMap.Length - 1}");
            }

            sbyte indexToMod = _indexMap[compID];

            if (indexToMod == -1) throw new ArgumentNullException($"can't cast a type that isn't defined in the archetype.");

            return (Module<T>)_dataMatrix[indexToMod];
      }

}