using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class HoopPool : Singleton<HoopPool>
{
    [Header("Prefabs")]
    [SerializeField] private NormalHoop normalPrefab;
    [SerializeField] private MovingHoop movingPrefab;
    [SerializeField] private SpikeHoop spikePrefab;

    [Header("Pool setting")]
    [SerializeField] private int defaultCapacity = 5;
    [SerializeField] private int maxSize = 20;

    private readonly Dictionary<HoopType, ObjectPool<HoopBase>> _pools = new();

    protected override void Awake()
    {
        base.Awake();


    }

    private void CreatePool(HoopType type, HoopBase prefab)
    {
        _pools[type] = new ObjectPool<HoopBase>(
            createFunc: () =>
            {
                HoopBase hoop = Instantiate(prefab, transform);

                hoop.gameObject.SetActive(false);

                return hoop;
            },


actionOnGet: hoop =>
{
    hoop.ResetState();
    hoop.gameObject.SetActive(true);
},

actionOnRelease: hoop =>
{
    hoop.gameObject.SetActive(false);
},

actionOnDestroy: hoop =>
{
    Destroy(hoop.gameObject);
},

collectionCheck: false,
defaultCapacity: defaultCapacity,
maxSize: maxSize
        );
    }

    public HoopBase Get(HoopType type)
    {
        if (!_pools.TryGetValue(type, out var pool)) return null;

        return pool.Get();
    }

    public void Release(HoopBase hoop)
    {
        if (hoop == null) return;

        if (!_pools.TryGetValue(hoop.Type, out var pool)) return;

        pool.Release(hoop);
    }
}