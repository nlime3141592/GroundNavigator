namespace System.Collections.Generic
{
    public static partial class ListExtension
    {
        public static void _CheckCapacity<T>(this List<T> array, int targetCapacity)
        {
            int currentCount = array.Count;

            for(int i = currentCount; i < targetCapacity; i++)
                array.Add(default(T));
        }
    }
}