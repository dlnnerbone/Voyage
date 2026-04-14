namespace Voyage;


// an interface that's used with classes that hold a unique ID, e.g, EntityID, ARchetypeID, etc.

/// <summary>
/// a simple interface for structs or classes that have an underlying, unique ID.
/// </summary>
/// <typeparam name="TReturn">the return type of the ID (e.g, int, ushort, byte, etc...</typeparam>
public interface IHasID<TReturn> : IHasID where TReturn : struct
{
      public new TReturn GetID();
}

/// <summary>
/// non-generic interface for classes holding an ID.
/// </summary>
public interface IHasID
{
      public object GetID();
}