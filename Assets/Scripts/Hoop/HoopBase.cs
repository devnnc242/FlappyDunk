using System;
using UnityEngine;

public abstract class HoopBase : MonoBehaviour, IHoop
{
    public static event Action<IHoop> OnHoopPassed;
    public static event Action<IHoop> OnHoopMissed;

    protected bool isScored;

    protected HoopScoredAnimation scoredAnimation;

    public abstract HoopType Type { get; }

    public GameObject Hoop => gameObject;

    public bool IsScored => isScored;

    protected virtual void Awake()
    {
        scoredAnimation = GetComponent<HoopScoredAnimation>();
    }

    public virtual void ResetState()
    {
        isScored = false;

        scoredAnimation?.ResetState();
    }

    public virtual void MarkScored()
    {
        if (isScored) return;

        isScored = true;

        OnHoopPassed?.Invoke(this);

        scoredAnimation?.Play();
    }

    public virtual void OnReadchedLimit()
    {
        if (!isScored)
        {
            OnHoopMissed?.Invoke(this);
        }

        HoopPool.Ins.Release(this);
    }

    public virtual void TickMovement()
    {

    }
}
