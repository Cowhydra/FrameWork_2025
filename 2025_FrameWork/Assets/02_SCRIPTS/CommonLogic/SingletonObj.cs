using UnityEngine;

public abstract class SingletonObj<T> : MonoBehaviour where T : SingletonObj<T>
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            if (object.ReferenceEquals(_Instance, null))
            {
                _Instance = FindAnyObjectByType(typeof(T)) as T;
                if (_Instance == null)
                {
                    GameObject obj = new GameObject("_" + typeof(T).ToString());
                    _Instance = obj.AddComponent<T>();
                }

                DontDestroyOnLoad(_Instance.gameObject);
            }

            return _Instance;
        }
    }

    public static bool IsValid
    {
        get { return (object.ReferenceEquals(_Instance, null) == false); }
    }

    public static void ExplicitInit()
    {
        Instance.Foo();
    }

    private void Foo() { }

    protected virtual void OnDestroy()
    {
        _Instance = null;
    }
}
