using Voyage.Helper;
using System.Runtime.InteropServices;
using System.Collections;
namespace Voyage.Operation;

public struct FastStack<T> : IEnumerable<T>
{
    internal T[] _buffer = null!;
    internal ushort nextIndex = 0;

    internal readonly int Count => nextIndex + 1;
    internal readonly int BufferSize => _buffer.Length;

    public readonly ref T this[int index] => ref _buffer[index];

    public FastStack()
    {
        _buffer = [];
    }

    public static FastStack<T> Create(int initialCount) => new FastStack<T>()
    {
        _buffer = new T[initialCount]
    };

    public static FastStack<T> Create(T[] _initialBuffer) => new FastStack<T>()
    {
        _buffer = _initialBuffer
    };

    public readonly Span<T> AsSpan() => _buffer.AsSpan(0, nextIndex);

    readonly IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public readonly FastStackEnumerator GetEnumerator() => new(this);

    public readonly ref T GetDataReference() => ref MemoryMarshal.GetArrayDataReference(_buffer);
    // methods
    internal readonly ref T Peek() => ref _buffer[nextIndex - 1];
    internal readonly ref T Peak() => ref _buffer[^1];

    public ushort Push(T comp)
    {
        if (_buffer == null || _buffer.Length == 0) _buffer = new T[4];
        else if (nextIndex > BufferSize - 1) ArrayHelper<T>.CopyAndResize(ref _buffer, BufferSize * 2);

        ushort returnVal = nextIndex++;
        _buffer[returnVal] = comp;
        return returnVal;
    }

    internal T Pop()
    {
        var buf = _buffer;
        T nextComp = buf[--nextIndex];

        if (RuntimeServices.IsReferenceOrContainsReferences<T>())
        {
            nextComp = default!;
        }
        return nextComp;
    }

    public int Compact()
    {
        int distanceLength = _buffer.Length - Count;

        ArrayHelper<T>.CopyAndResize(ref _buffer, nextIndex);
        return distanceLength;
    }

    public struct FastStackEnumerator : IEnumerator<T>
    {
        T[] buffer;
        int iterator = -1;
        readonly int max;

        public bool MoveNext() => ++iterator < max;
        public void Reset() => iterator = -1;
        public void Dispose() => buffer = null!;

        public readonly T Current => buffer[iterator];
        readonly object IEnumerator.Current => Current!;

        internal FastStackEnumerator(in FastStack<T> stack)
        {
            buffer = stack._buffer;
            max = stack.nextIndex;
        }
    }
}