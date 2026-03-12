namespace Voyage;

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