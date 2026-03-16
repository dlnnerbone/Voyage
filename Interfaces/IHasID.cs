namespace Voyage;


// an interface that's used with classes that hold a unique ID, e.g, EntityID, ARchetypeID, etc.

/// <summary>
/// a simple interface for structs or classes that have an underlying, unique ID.
/// </summary>
/// <typeparam name="TReturn">the return type of the ID (e.g, int, ushort, byte, etc...</typeparam>
public interface IHasID<TReturn> where TReturn : struct
{
      public TReturn GetID();
}