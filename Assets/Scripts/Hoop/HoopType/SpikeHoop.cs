using UnityEngine;

public class SpikeHoop : HoopBase
{
    [SerializeField] private GameObject leftSpike;
    [SerializeField] private GameObject rightSpike;

    public override HoopType Type => HoopType.Spike;


    public override void ResetState()
    {
        base.ResetState();

        leftSpike.SetActive(true);
        rightSpike.SetActive(false);
    }
}