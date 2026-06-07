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

public interface IUpdatable<T>
{
    void Update(ref T component);
}