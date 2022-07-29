namespace System.Collections.Generic
{
    public static partial class ListExtension
    {
        public static void CheckCapacity<T>(this List<T> array, int targetCapacity)
        {
            int currentCapacity = array.Capacity;

            for(int i = currentCapacity; i < targetCapacity; i++)
                array.Add(default(T));
        }
    }
}