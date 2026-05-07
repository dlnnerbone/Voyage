namespace Voyage;

public partial struct Entity : INull, IHasID<int>
{
      internal int ID;
      internal ushort ArchetypeID;
      internal ushort Queue;

      public Entity() {}

      internal Entity(int id, ushort archID, ushort queuePos)
      {
            ID = id;
            ArchetypeID = archID;
            Queue = queuePos;
      }

      public readonly struct Reader(in Entity entity)
      {
            public readonly int ID = entity.ID;
            public readonly ushort ArchetypeID = entity.ArchetypeID;
            public readonly ushort Queue = entity.Queue;

            public static Reader GetInfo(Entity entity) => new(entity);
      }

      public readonly Reader Read() => new(this);
      public static Entity Null => new(-1, 0, 0);

      public readonly int GetID() => ID;
      readonly object IHasID.GetID() => GetID();

      public static implicit operator Reader(Entity ent) => ent.Read();
}