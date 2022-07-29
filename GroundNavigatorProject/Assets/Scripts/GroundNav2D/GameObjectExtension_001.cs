namespace UnityEngine
{
    public static partial class GameObjectExtension
    {
        public static T ValidateComponent<T>(this GameObject obj)
        where T: UnityEngine.Component
        {
            T component;
            bool canGetComponent = obj.TryGetComponent<T>(out component);

            if(canGetComponent)
                return component;
            else
                return obj.AddComponent<T>();
        }
    }
}