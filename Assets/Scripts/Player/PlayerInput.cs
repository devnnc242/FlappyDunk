using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    //public event Action OnTap;
    public UnityEvent OnTap;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnTap?.Invoke();
        }
#endif

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                OnTap?.Invoke();
            }
        }
    }
}
