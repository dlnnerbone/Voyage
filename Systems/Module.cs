namespace Voyage.Operation;

public class Module<T> : IModule<T>, IModule
{
      internal T[] _buffer;
      internal ushort[] _denseSet = Array.Empty<ushort>();
      internal byte[] _sparseSet;

      public ref T[] GetBuffer() => ref _buffer;

      internal Module(uint initialAmount)
      {
            _buffer = new T[initialAmount];
            _sparseSet = new byte[initialAmount];

            _sparseSet.AsSpan().Fill(0);
      }

      public void Toggle(uint index, byte toggle)
      {
            if (index > _buffer.Length - 1) throw new ArgumentOutOfRangeException($"{index} is out of bounds of array.");
            else if (_sparseSet[index] == toggle) return;
            // the above line checks if the byte is already toggled by the choice, and returns the method to prevent the code below
            // from running every frame.
            _sparseSet[index] = toggle;
            Refresh();
      }

      public void ToggleSelection(IEnumerable<uint> indices, byte toggle)
      {
            uint[] toArr = indices.ToArray();

            for(int i = 0; i < toArr.Length; i++)
            {
                  ref readonly var index = ref toArr[i];

                  if (index > _sparseSet.Length - 1) throw new ArgumentOutOfRangeException($"{i} is out of bounds of the array.");
                  else if (_sparseSet[index] == toggle) continue;

                  _sparseSet[i] = toggle;
            }

            Refresh();
      }

      // despite this being public, let the Module (or archetypes) handle it.
      public void Refresh()
      {
            uint length = 0;
            for(int i = 0; i < _buffer.Length; i++)
            {
                  if (_sparseSet[i] == 1) ++length;
            }

            _denseSet = new ushort[length];
            uint tracker = 0;
            for(ushort i = 0; i < _buffer.Length; i++)
            {
                  if (_sparseSet[i] == 1) _denseSet[tracker++] = i;
            }
      }
}