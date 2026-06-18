using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _ins;

    public static T Ins
    {
        get
        {
            if (_ins == null)
            {
                _ins = FindFirstObjectByType<T>();

                if (_ins == null)
                {
                    Debug.LogError($"Singleton {typeof(T).Name} not found in scene");
                }
            }

            return _ins;
        }
    }

    protected virtual void Awake()
    {
        if (_ins == null)
        {
            _ins = this as T;

            if (DontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (_ins != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual bool DontDestroy => false;
}
