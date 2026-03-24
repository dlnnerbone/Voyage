namespace Voyage.Helper;

public static class ArrayHelper<T>
{
      public static T[] Copy(T[] source)
      {
            T[] destination = new T[source.Length];
            for(int i = 0; i < source.Length; i++) destination[i] = source[i];
            return destination;
      }

      public static T[] Copy(T[] source, int start)
      {
            if (start > source.Length - 1) throw new ArgumentOutOfRangeException($"{start} is out of bounds of array's buffer size.");
            int distanceLength = source.Length - start;

            T[] destination = new T[distanceLength];
            for(int i = 0; i < distanceLength; i++)
            {
                  int sourceIndex = start + i;
                  destination[i] = source[sourceIndex];
            }

            return destination;
      }

      public static void Resize(ref T[] source, int newLength) => source = new T[newLength];

      /// <summary>
      /// method that both resizes and copies elements into a new array.
      /// note: if source is less than the new length, the iterator will use the minimum instead.
      /// </summary>
      /// <param name="source">the source array to change.</param>
      /// <param name="newLength">the new length for the array.</param>
      public static void CopyAndResize(ref T[] source, int newLength)
      {
            T[] newArr = new T[newLength];
            int min = Math.Min(newLength, source.Length);

            for(int i = 0; i < min; i++) newArr[i] = source[i];
            source = newArr;
      }
}