using Voyage;
using Voyage.Operation;
namespace Voyage.Marshals;

public static class EntityMarshal
{
    public static Entity CreateEntity(int id, ushort archID, ushort queuePos) => new(id, archID, queuePos);
    public static void SetID(ref Entity ent, int newID) => ent.ID = newID;
    public static void SetArchID(ref Entity ent, ushort newArchID) => ent.ArchetypeID = newArchID;
    public static void SetQueue(ref Entity ent, ushort newQueue) => ent.Queue = newQueue;
}