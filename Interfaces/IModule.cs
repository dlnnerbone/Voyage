namespace Voyage;

internal interface IModule
{
      public void Refresh();
      public void Toggle(uint index, byte toggle);
      public void ToggleSelection(IEnumerable<uint> group, byte toggle);
}

internal interface IModule<T> : IModule
{
      public T[] GetBuffer();
}