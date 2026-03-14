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

public interface IDynamicUpdatable<TReturn, T> : IUpdatable
{
    TReturn Update(in T component);
}

public interface IDynamicMutableUpdatable<TReturn, T> : IUpdatable
{
    TReturn Update(ref T component);
}