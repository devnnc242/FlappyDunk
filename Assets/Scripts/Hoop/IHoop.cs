using UnityEngine;

/// <summary>
/// Contract for all hoop types in the game
/// </summary>
public interface IHoop
{
    //Identity
    GameObject Hoop { get; }

    //State
    bool IsScored { get; }

    //Lifecycle
    /// <summary>Reset all visual</summary>
    void ResetState();

    /// <summary>Through the hoop </summary>
    void MarkScored();

    //Limit
    void OnReadchedLimit();
}
