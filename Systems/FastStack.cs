using Voyage.Helper;
using System.Runtime.InteropServices;
using System.Collections;
namespace Voyage.Operation;

public struct FastStack<T> : IEnumerable<T>
{
    internal T[] _buffer = null!;
    internal int nextIndex = 0;

    internal readonly int Count => nextIndex + 1;
    internal readonly int BufferSize => _buffer.Length;

    public readonly ref T this[int index] => ref _buffer[index];

    public FastStack() {}

    public static FastStack<T> Create(int initialCount) => new FastStack<T>()
    {
        _buffer = new T[initialCount]
    };

    public static FastStack<T> Create(T[] _initialBuffer) => new FastStack<T>()
    {
        _buffer = _initialBuffer
    };

    public readonly Span<T> AsSpan() => _buffer.AsSpan(0, nextIndex);

    public readonly IEnumerator<T> GetEnumerator() => new FastStackEnumerator(this);
    readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // methods
    internal readonly ref T Peek() => ref _buffer[nextIndex - 1];
    internal readonly ref T Peak() => ref _buffer[^1];

    internal void Push(T comp)
    {
        if (_buffer == null) _buffer = new T[4];
        else if (nextIndex > BufferSize - 1) ArrayHelper<T>.CopyAndResize(ref _buffer, BufferSize * 2);

        _buffer[nextIndex++] = comp;
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

    internal struct FastStackEnumerator : IEnumerator<T>
    {
        T[] buffer;
        int iterator = -1;
        readonly int max;

        readonly T IEnumerator<T>.Current => buffer[iterator];
        public bool MoveNext() => ++iterator > max;
        public void Reset() => iterator = -1;
        public readonly object Current => Current;
        public void Dispose() => buffer = null!;

        internal FastStackEnumerator(in FastStack<T> stack)
        {
            buffer = stack._buffer;
            max = stack.nextIndex;
        }
    }
}