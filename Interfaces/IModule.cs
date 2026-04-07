namespace Voyage;

public interface IModule
{
      public void Refresh();
      public void ToggleElement(ushort index, bool toggle);
      public void ToggleElementSelection(ushort[] group, bool toggle);
}

public interface IModule<T> : IModule
{
      public T[] GetBuffer();
}