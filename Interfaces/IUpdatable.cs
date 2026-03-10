namespace Voyage;

public interface IUpdatable
{
    void Update();
}

public interface IUpdatable<T>
{
    void Update(in T component);
}

public interface IUpdatableMutable<T>
{
    void Update(ref T component);
}