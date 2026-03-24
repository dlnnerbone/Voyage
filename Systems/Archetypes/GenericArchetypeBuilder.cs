namespace Voyage.Operation;

public class ArchetypeBuilder : IArchetypeBuilder
{
      internal Archetype _archetype = Archetype.Null;

      internal object[] _modules;
      internal string _collectedTypes;
      internal sbyte[] _indexMap;
      internal Type[] _types;

      private ushort _typesCounted = 0;

      public ArchetypeBuilder(int fixedCount)
      {
            _archetype.TypeCount = fixedCount;
            _modules = new object[fixedCount];
            _types = new Type[fixedCount];
      }

      public ArchetypeBuilder Implement<T>()
      {
            Type typeOfGeneric = typeof(T);
            ushort curIndex = _typesCounted++;
            ushort compID = ComponentMetadata<T>.ID;

            if (curIndex > _archetype.TypeCount - 1)
            {
                  throw new ArgumentOutOfRangeException($"too many types have been implemented beyond the type buffer: {curIndex}");
            }
            else if (compID > _indexMap.Length - 1)
            {
                  
            }

            return this;
      }

      public Archetype Return() => _archetype;
}