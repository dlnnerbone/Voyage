namespace Voyage;

internal interface IModule
{
      void Refresh();
      void Toggle(uint index, bool toggle);
      void ToggleSelection(IEnumerable<uint> group, bool toggle);
}