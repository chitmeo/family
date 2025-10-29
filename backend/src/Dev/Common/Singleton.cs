namespace Dev.Common;

internal class Singleton<T> : BaseSingleton
{
    private static T? _instance;

    /// <summary>
    /// The singleton instance for the specified type T. Only one instance (at the time) of this object for each type of T.
    /// </summary>
    public static T Instance
    {
        get => _instance!;
        set
        {
            _instance = value;
            if (value != null)
            {
                AllSingletons[typeof(T)] = value;
            }
            else
            {
                AllSingletons.Remove(typeof(T));
            }
        }
    }
}
