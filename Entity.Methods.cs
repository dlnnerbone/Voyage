using Voyage.Operation;
namespace Voyage;

public partial struct Entity : IEquatable<Entity>
{
      public override readonly int GetHashCode() => ID + ArchetypeID + Queue;
      public readonly bool IsNull() => ID == 0;
      public static bool IsNull(Entity entity) => entity.IsNull();

      public readonly ref TComp Get<TComp>(World world)
      {
            if (IsNull()) throw new NullReferenceException($"Entity that is 'null' can not be used to attain a component by reference.");
            
            Archetype arch = world.GetArchetype(ArchetypeID);
            
            if (arch.IsNull()) throw new NullReferenceException($"Getting an archetype that is 'null' is invalid.");

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

      public readonly override string ToString() => $"ID: {ID}, Archetype: {ArchetypeID}, Position: {Queue}";

      internal void SetID(int newID) => ID = newID;
      internal void SetArch(ushort archID) => ArchetypeID = archID;
      internal void SetQueue(ushort newQueue) => Queue = newQueue;
}