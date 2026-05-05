using Voyage.Helper;
namespace Voyage.Operation;

public class ArchetypeBuilder : IArchetypeBuilder
{
      internal Archetype _archetype = Archetype.Null;

      internal object[] _modules = [];
      internal string _collectedTypes = null!;
      internal byte[] _indexMap = [];
      internal Type[] _types = [];
      private Action<int> _resizer;

      private byte _typesCounted = 0;

      public ArchetypeBuilder(int fixedCount, int capacity)
      {
            _archetype.TypeCount = fixedCount;
            _archetype.Capacity = capacity;

            _modules = new object[fixedCount];
            _types = new Type[fixedCount];
            _resizer = (length) => {};
      }

      public ArchetypeBuilder Implement<T>()
      {
            Type typeOfGeneric = typeof(T);
            byte curIndex = _typesCounted++;
            ushort compID = ComponentMetadata<T>.ID;

            if (curIndex > _archetype.TypeCount - 1)
            {
                  throw new ArgumentOutOfRangeException($"too many types have been implemented beyond the type buffer: {curIndex}");
            }
            else if (compID > _indexMap.Length - 1) ArrayHelper<byte>.CopyAndResize(ref _indexMap, compID + 4);

            if (_types[_indexMap[compID]] == typeOfGeneric) throw new ArgumentException($"{typeOfGeneric} is already defined in archetype.");

            _types[curIndex] = typeOfGeneric;
            _indexMap[compID] = curIndex;
            _modules[curIndex] = new Module<T>(_archetype.Capacity);

            var mod = (Module<T>)_modules[curIndex];
            _resizer += mod.ResizeModule;
            return this;
      }

      public ArchetypeBuilder Finalize()
      {
            _archetype.TypeCount = _typesCounted + 1;
            _archetype._typeSet = [.. _types];
            _archetype._dataMatrix = _modules;
            _archetype._collectedTypes = string.Join(", ", _archetype._typeSet);
            _archetype._indexMap = _indexMap;
            _archetype._entityMap = new int[_archetype.Capacity];
            _archetype._archetypeResizer = _resizer;
            return this;
      }

      public Archetype Return() => _archetype;
}