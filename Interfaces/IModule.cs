namespace Voyage;

public interface IModule
{
      public void Refresh();
      public void ToggleElement(int index, bool toggle);
      public void ToggleElementSelection(int[] group, bool toggle);
}

public interface IModule<T> : IModule
{
      public T[] GetBuffer();
}