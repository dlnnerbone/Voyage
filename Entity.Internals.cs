namespace Voyage;

public partial struct Entity : INull
{
      internal int EntityID;
      internal ushort ArchetypeID;
      internal ushort Queue;

      public readonly struct EntityReader
      {
            public readonly int EntityID;
            public readonly ushort ArchetypeID;
            public readonly ushort Queue;

            public EntityReader(Entity entity)
            {
                  EntityID = entity.EntityID;
                  ArchetypeID = entity.ArchetypeID;
                  Queue = entity.Queue;
            }

            public static EntityReader GetInfo(Entity entity) => new(entity);
      }

      public readonly struct EntityLookup
      {
            public readonly ushort ArchetypeID;
            public readonly ushort Queue;

            public EntityLookup(Entity entity)
            {
                  ArchetypeID = entity.ArchetypeID;
                  Queue = entity.Queue;
            }

            public static EntityLookup GetLookup(in Entity ent) => new(ent);
      }

      public readonly EntityReader Read() => new(this);
      public readonly EntityLookup GetEntityLookup() => new(this);

      public static implicit operator EntityReader(Entity ent) => ent.Read();
      public static implicit operator EntityLookup(Entity ent) => ent.GetEntityLookup();
}