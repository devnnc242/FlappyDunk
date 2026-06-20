using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //public event Action OnTap;
    public event Action OnTap;

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnTap?.Invoke();
        }
#endif

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            OnTap?.Invoke();
        }
    }
}
