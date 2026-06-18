using UnityEngine;

public abstract class HoopBase : MonoBehaviour, IHoop
{
    protected bool isScored;
    public GameObject Hoop => gameObject;
    public bool IsScored => isScored;

    public virtual void ResetState()
    {
        isScored = false;
    }

    public virtual void MarkScored()
    {
        isScored = true;
    }

    public virtual void OnReadchedLimit()
    { }
}
