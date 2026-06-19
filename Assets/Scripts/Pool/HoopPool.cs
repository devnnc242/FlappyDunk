using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HoopPool : Singleton<HoopPool>
{
    [Header("Prefabs")]
    [SerializeField] private NormalHoop normalPrefab;
    [SerializeField] private MovingHoop movingPrefab;
    [SerializeField] private SpikeHoop spikePrefab;

    private readonly Dictionary<HoopType, ObjectPool<HoopBase>> _pools = new();

    protected override void Awake()
    {
        base.Awake();


    }

    private void CreatePool(HoopType type, HoopBase prefab)
    {
        _pools[type] = new ObjectPool<HoopBase>(
            () =>
            {
                HoopBase hoop = Instantiate(prefab, transform);

                hoop.gameObject.SetActive(false);

                return hoop;
            },


hoop =>
{
    hoop.ResetState();
    hoop.gameObject.SetActive(true);
},

hoop =>
{
    hoop.gameObject.SetActive(false);
},

hoop =>
{
    Destroy(hoop.gameObject);
},

false,
5,
20
        );
    }

    public HoopBase Get(HoopType type)
    {
        return _pools[type].Get();
    }

    public void Release(HoopBase hoop)
    {
        _pools[hoop.Type].Release(hoop);
    }
}