namespace Voyage;

public partial struct Entity : IHasID<int>, IEquatable<Entity>
{
      // not recommended to create an entity without world.
      public Entity() {}

      /// <summary>
      /// the internal entity constructor.
      /// </summary>
      /// <param name="worldID">the unique id an entity occupies.</param>
      /// <param name="archetypeID">the indexer for what archetype and entity corresponds to.</param>
      /// <param name="queue">the queue of where the entity is within the archetype.</param>
      internal Entity(int worldID, ushort archetypeID, ushort queue)
      {
            EntityID = worldID;
            ArchetypeID = archetypeID;
            Queue = queue;
      }

      // creates a 'null' entity (no archetype or position)
      public static Entity Null => new Entity(-1, 0, 0);

      public int GetID() => EntityID;

      public override int GetHashCode() => HashCode.Combine(EntityID, ArchetypeID, Queue);
      public bool IsNull() => EntityID == -1;
      public static bool IsNull(in Entity entity) => entity.IsNull();

      // operators

      public bool Equals(Entity other)
      {
            (ushort, ushort) ver = (ArchetypeID, Queue), ver2 = (other.ArchetypeID, other.Queue);
            return ver == ver2;
      }

      public static bool Equals(Entity main, Entity other) => main.Equals(other);
      
}