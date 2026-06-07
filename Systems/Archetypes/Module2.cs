using System.Runtime.CompilerServices;
using System.Collections.Immutable;
namespace Voyage.Operation;

public class Module<T> : IModule<T>
{
    // an array of the chosen components.
    internal T[] _compBuffer;
    // Sparse Set values are used to access dense set indices
    internal int[] _activeComponentsSparse = [];
    // dense set that access active components
    internal int[] _activeComponentsDense = [];
    // hat components are active.
    internal bool[] _activatedElements;
    internal int denseCount = 0;

    public ref T this[int id] => ref _compBuffer[id];
    public ref T AttainByDense(int id) => ref _compBuffer[_activeComponentsDense[id]];

    public int Length => _compBuffer.Length;
    public int DenseCount => denseCount;
    public T[] GetBuffer() => _compBuffer;

    public ImmutableArray<int> GetSparseSet() => [.. _activeComponentsSparse ];
    public ImmutableArray<int> GetDenseSet() => [ .. _activeComponentsDense ];
    public ImmutableArray<bool> ActivatedElements() => [.. _activatedElements ];

    public Module(int initLength)
    {
        _compBuffer = new T[initLength];
        _activeComponentsSparse = new int[initLength];
        _activatedElements = new bool[initLength];
    }

    public Module(T[] initBuffer)
    {
        _compBuffer = initBuffer;
        _activeComponentsSparse = new int[initBuffer.Length];
        _activatedElements = new bool[initBuffer.Length];
    }

    public bool IsActive(int index) => _activatedElements[index];

    public void ToggleElement(int index, bool mode)
    {
        if (index > Length - 1) throw new ArgumentOutOfRangeException($"index: {index} is out of bounds of the module.");
        
        int setter = mode ? 1 : -1;
        if (_activatedElements[index] != mode)
        {
            _activatedElements[index] = mode;
            denseCount += setter;
            Refresh();
        }
    }

    public void Refresh()
    {
        _activeComponentsDense = new int[denseCount];

        ushort _denseCounter = 0;
        for(int i = 0; i < Length; i++)
        {
            if (!_activatedElements[i]) continue;

            var next = _denseCounter++;
            _activeComponentsDense[next] = i;
            _activeComponentsSparse[i] = next;
        }
    }

    public void ResizeModule(int newLength)
    {
        denseCount = 0;
        bool isLower = newLength < Length;
        var min = Math.Min(Length, newLength);

        T[] newBuffer = new T[newLength];
        _activeComponentsSparse = new int[newLength];
        bool[] _activatedElems = new bool[newLength];

        for(int i = 0; i < min; i++)
        {
            newBuffer[i] = _compBuffer[i];

            if (!_activatedElements[i]) continue;

            ++denseCount;
            _activatedElems[i] = _activatedElements[i];
        }

        _compBuffer = newBuffer;
        _activatedElements = _activatedElems;
        if (isLower) Refresh();
    }

    public void ToggleElementSelection(int[] indices, bool mode)
    {
        int setter = mode ? 1 : -1;
        var prevDenseCount = denseCount;

        for(int i = 0; i < indices.Length; i++)
        {
            if (indices[i] > Length - 1) throw new ArgumentOutOfRangeException($"at index: {i}, of value: {indices[i]} is out of bounds of module.");
            else if (_activatedElements[indices[i]] == mode) continue;

            denseCount += setter;
            _activatedElements[indices[i]] = mode; 
        }

        if (prevDenseCount != denseCount) Refresh();
    }

}