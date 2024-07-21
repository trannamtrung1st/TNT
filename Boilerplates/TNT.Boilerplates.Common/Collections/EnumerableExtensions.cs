namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static T[] SubSet<T>(this T[] arr, int fromIdx, int length)
        {
            var newArr = new T[length];

            Array.Copy(arr, fromIdx, newArr, 0, length);

            return newArr;
        }
    }
}
