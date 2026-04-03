namespace Voyage;

public interface IModule
{
      public void Refresh();
      public void Toggle(uint index, byte toggle);
      public void ToggleSelection(uint[] group, byte toggle);
}

public interface IModule<T> : IModule
{
      public T[] GetBuffer();
}