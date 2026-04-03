namespace Voyage;

public partial struct Entity
{
      internal int EntityID;
      internal ushort ArchetypeID;
      internal ushort Queue;

      public struct EntityReader
      {
            public readonly int EntityID;
            public readonly ushort ArchetypeID;
            public readonly ushort Queue;

            public EntityReader(in Entity entity)
            {
                  EntityID = entity.EntityID;
                  ArchetypeID = entity.ArchetypeID;
                  Queue = entity.Queue;
            }

            public static EntityReader GetInfo(Entity entity) => new(entity);
      }

      public struct EntityLookup
      {
            public readonly ushort ArchetypeID;
            public readonly ushort Queue;

            public EntityLookup(in Entity entity)
            {
                  ArchetypeID = entity.ArchetypeID;
                  Queue = entity.Queue;
            }

            public static EntityLookup GetLookup(in Entity ent) => new(ent);
      }

      public readonly EntityReader Read() => new(this);
      public readonly EntityLookup GetEntityLookup() => new(this);
}