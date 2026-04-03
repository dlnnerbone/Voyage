using System.Collections.Immutable;
using Voyage.Helper;
using System.Runtime.CompilerServices;
namespace Voyage.Operation;

public class Module<T> : IModule<T>, IModule
{
      internal T[] _buffer;
      internal ushort[] _denseSet = Array.Empty<ushort>();
      internal byte[] _sparseSet;

      public T[] GetBuffer() => _buffer;
      public ImmutableArray<byte> GetSparse() => _sparseSet.ToImmutableArray();
      public ImmutableArray<ushort> GetDense() => _denseSet.ToImmutableArray();
      public ref T this[int id] => ref _buffer[id];
      internal ref T GetAsUnsafe(int id) => ref Unsafe.AsRef(ref _buffer[id]);
       
      internal Module(uint initialAmount)
      {
            _buffer = new T[initialAmount];
            _sparseSet = new byte[initialAmount];

            _sparseSet.AsSpan().Clear();
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

      public void ToggleSelection(uint[] indices, byte toggle)
      {
            for(int i = 0; i < indices.Length; i++)
            {
                  ref readonly var index = ref indices[i];

                  if (index > _sparseSet.Length - 1) throw new ArgumentOutOfRangeException($"{i} is out of bounds of the array.");
                  else if (_sparseSet[index] == toggle) continue;

                  _sparseSet[i] = toggle;
            }

            Refresh();
      }

      public void Resize(int newLength)
      {
            ArrayHelper<T>.CopyAndResize(ref _buffer, newLength);
            ArrayHelper<byte>.CopyAndResize(ref _sparseSet, newLength);
            
            Refresh();
      }

      // despite this being public, let the Module (or archetype) handle it.
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

      public static Module<T> Create(uint initialAmount) => new Module<T>(initialAmount);
}