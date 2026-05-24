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
                    GameObject singleton = new GameObject(typeof(T).Name);
                    _ins = singleton.AddComponent<T>();
                }
            }

            return _ins;
        }
    }

    protected virtual void Awake()
    {

    }

    public void MakeSingleton(bool destroyOnLoad)
    {
        if (_ins == null)
        {
            _ins = this as T;

            if (destroyOnLoad)
            {
                var root = transform.root;

                if (root != transform)
                {
                    DontDestroyOnLoad(root);
                }
                else
                {
                    DontDestroyOnLoad(this.gameObject);
                }
            }
        }
        else if (_ins == this)
        {
            return;
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
