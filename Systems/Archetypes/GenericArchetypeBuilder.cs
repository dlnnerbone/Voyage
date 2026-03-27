using Voyage.Helper;
namespace Voyage.Operation;

public class ArchetypeBuilder : IArchetypeBuilder
{
      internal Archetype _archetype = Archetype.Null;

      internal object[] _modules;
      internal string _collectedTypes = null!;
      internal byte[] _indexMap = Array.Empty<byte>();
      internal Type[] _types;

      private sbyte _typesCounted = 0;
      private uint _cap;

      public ArchetypeBuilder(int fixedCount, uint capacity)
      {
            _archetype.TypeCount = fixedCount;
            _modules = new object[fixedCount];
            _types = new Type[fixedCount];

            _cap = capacity;
      }

      public ArchetypeBuilder Implement<T>()
      {
            Type typeOfGeneric = typeof(T);
            sbyte curIndex = _typesCounted++;
            ushort compID = ComponentMetadata<T>.ID;

            if (curIndex > _archetype.TypeCount - 1)
            {
                  throw new ArgumentOutOfRangeException($"too many types have been implemented beyond the type buffer: {curIndex}");
            }
            else if (compID > _indexMap.Length - 1) ArrayHelper<sbyte>.CopyAndResize(ref _indexMap, compID + 4);

            if (_types[_indexMap[compID]] == typeOfGeneric) throw new ArgumentException($"{typeOfGeneric} is already defined in archetype.");

            _types[curIndex] = typeOfGeneric;
            _indexMap[compID] = curIndex;
            _modules[curIndex] = new Module<T>(_cap);

            return this;
      }

      public ArchetypeBuilder Finalize(int id)
      {
            _archetype.ArchetypeID = id;
            _archetype.TypeCount = _typesCounted + 1;
            _archetype._typeSet = _types.ToHashSet();
            _archetype._dataMatrix = _modules;
            _archetype._collectedTypes = string.Join(", ", _archetype._typeSet);
            _archetype._indexMap = _indexMap;
            return this;
      }

      public Archetype Return() => _archetype;
}