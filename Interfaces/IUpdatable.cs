namespace Voyage;

// the IUpdatable interfaces.


/// <summary>
/// the base IUpdatable interface for classes that need an update per-frame method, it's recommended to put other update methods inside here,
/// unless you're using generics.
/// </summary>
public interface IUpdatable
{
    void Update();
}

public interface IUpdatable<T> : IUpdatable
{
    void Update(in T component);
}

public interface IUpdatableMutable<T> : IUpdatable
{
    void Update(ref T component);
}