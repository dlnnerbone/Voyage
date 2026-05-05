using System.Collections.Immutable;
using Voyage.Helper;
using System.Runtime.CompilerServices;
using System.Collections;
namespace Voyage.Operation;

public class Module<T> : IModule<T>, IEnumerable<T>
{
      internal T[] _buffer;
      internal ushort[] _denseSet = [];
      internal byte[] _sparseSet;

      public ref T this[int index] => ref _buffer[index];
      public ref T AttainByQueue(int index) => ref _buffer[_denseSet[index]];
      
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
                  byte index = _sparseSet[i];
                  if (index == 1) ++amountOfValid;
            }

            _denseSet = new ushort[amountOfValid];
            ushort indexTracker = 0;
            for(ushort i = 0; i < _sparseSet.Length; i++)
            {
                  byte indexValue = _sparseSet[i];
                  if (indexValue == 1) _denseSet[indexTracker++] = i; 
            }
      }

      public bool IsElementValid(int index) => _sparseSet[index] == 1;

      public bool AnyElementsValid(ushort[] indices)
      {
            if (indices.Length == 0) return false;

            for(int i = 0; i < indices.Length; i++)
            {
                  if (_sparseSet[indices[i]] == 1) return true;
            }
            return false;
      }

      public bool AllElementsValid(ushort[] indices)
      {
            if (indices.Length == 0) return false;

            for(int i = 0; i < indices.Length; i++)
            {
                  if (_sparseSet[indices[i]] == 0) return false;
            }
            return true;
      }
      
      internal void ResizeModule(int newLength)
      {
            T[] newBuffer = new T[newLength];
            byte[] newSparseBuffer = new byte[newLength];
            int previousLength = this.Length;
            
            int min = Math.Min(Length, newLength);

            for(int i = 0; i < min; i++)
            {
                  newBuffer[i] = _buffer[i];
                  newSparseBuffer[i] = _sparseSet[i];
            }

            _sparseSet = newSparseBuffer;
            _buffer = newBuffer;

            if (min < previousLength) Refresh();
      }

      public ModuleEnumerator GetEnumerator() => new(this);
      IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      public struct ModuleEnumerator : IEnumerator<T>
      {
            internal Module<T> module;
            internal ushort[] _denseSet;
            internal int denseLength;
            internal int position = -1;

            internal ModuleEnumerator(Module<T> module)
            {
                  this.module = module;
                  _denseSet = module.GetDenseSet();
                  denseLength = module._denseSet.Length;
                  position = -1;
            }

            public void Reset() => position = -1;
            public bool MoveNext() => ++position < denseLength;
            public void Dispose()
            {
                  module = null!;
                  _denseSet = null!;
                  denseLength = -1;
                  position = -1;
            }

            public readonly T Current => module[_denseSet[position]];
            readonly object IEnumerator.Current => Current!; 
      }

}