using Voyage.Operation;
namespace Voyage;

public partial struct Entity : IEquatable<Entity>
{
      public override readonly int GetHashCode() => HashCode.Combine(ArchetypeID, Queue);
      public readonly bool IsNull() => ArchetypeID == 0;
      public static bool IsNull(Entity entity) => entity.IsNull();

      public ref TComp Get<TComp>()
      {
            var arch = PrimaryWorld._world._query[ArchetypeID];
            return ref arch.GetComponent<TComp>(Queue);
      }

      // operators

      public readonly bool Equals(Entity other) 
      {
            (ushort, ushort) check = (ArchetypeID, Queue);
            (ushort, ushort) check2 = (other.ArchetypeID, other.Queue);
            return check == check2;
      }

      public static bool Equals(Entity main, Entity other) => main.Equals(other);
      
}