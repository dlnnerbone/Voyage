namespace Voyage.Operation;

public class Module<T> : IModule
{
      internal T[] _buffer;
      internal ushort[] _denseSet = Array.Empty<ushort>();
      internal byte[] _sparseSet;

      internal Module(uint initialAmount)
      {
            _buffer = new T[initialAmount];
      }
}