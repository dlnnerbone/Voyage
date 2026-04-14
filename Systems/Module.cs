using System.Collections.Immutable;
using Voyage.Helper;
using System.Runtime.CompilerServices;
namespace Voyage.Operation;

public class Module<T> : IModule<T>, IEnumerable<T>
{
      internal T[] _buffer;
      internal ushort[] _denseSet = [];
      internal byte[] _sparseSet;

      public ref T this[int index] => ref _buffer[index];
      public T[] GetBuffer() => _buffer;
      public ushort[] GetDenseSet() => _denseSet;
      public byte[] GetSparseSet() => _sparseSet;

      public bool HasElements() => _buffer.Length > 0;
      public int Length => _buffer.Length;

      public Module(int initialCount)
      {
            _buffer = new T[initialCount];
            _sparseSet = new byte[initialCount];
      }

      public Module(T[] _initialBuffer)
      {
            _buffer = _initialBuffer;
            _sparseSet = new byte[_buffer.Length];
      }

      public void ToggleElement(ushort indexToElement, bool toggle)
      {
            if (indexToElement > Length - 1)
            {
                  throw new ArgumentOutOfRangeException($"{indexToElement} is out of bounds of buffer.");
            }
            
            var convertValue = Unsafe.BitCast<bool, byte>(toggle);
            if (_sparseSet[indexToElement] != convertValue)
            {
                  _sparseSet[indexToElement] = convertValue;
                  Refresh();
            }
      }

      public void ToggleElementSelection(ushort[] indices, bool toggle)
      {
            bool queue = false;
            var convertValue = Unsafe.BitCast<bool, byte>(toggle);

            for(int i = 0; i < indices.Length; i++)
            {
                  if (indices[i] > Length - 1) throw new ArgumentOutOfRangeException($"at index {i}, {indices[i]}, is out of bounds.");

                  ref var index = ref _sparseSet[indices[i]];
                  if (index != convertValue) queue = true;
                  index = convertValue; 
            }
            if (queue) Refresh();
      }

      public void Refresh()
      {
            uint amountOfValid = 0;

            // this loop reads the array and increments the above field as it passes by bytes with values of 1.
            for(int i = 0; i < _sparseSet.Length; i++)
            {
                  ref readonly byte index = ref _sparseSet[i];
                  if (index == 1) ++amountOfValid;
            }

            _denseSet = new ushort[amountOfValid];
            ushort indexTracker = 0;
            for(ushort i = 0; i < _sparseSet.Length; i++)
            {
                   ref readonly byte indexValue = ref _sparseSet[i];
                   if (indexValue == 1) _denseSet[indexTracker++] = i; 
            }
      }

      // resizing module.
      internal void ResizeModule(int newLength)
      {
            T[] newBuffer = new T[newLength];
            byte[] newSparseBuffer = new byte[newLength];
            int previousLength = this.Length;
            
            int min = Math.Min(Length, newLength);

            for(int i = 0; i < min; i++)
            {
                  ref readonly T element = ref _buffer[i];

                  newBuffer[i] = element;
                  newSparseBuffer[i] = _sparseSet[i];
            }

            _sparseSet = newSparseBuffer;
            _buffer = newBuffer;

            if (min < previousLength) Refresh();
      }

      // enumeration

      public IEnumerator<T> GetEnumerator()
      {
            for(int i = 0; i < _buffer.Length; i++)
            {
                  yield return _buffer[i];
            }
      }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

}