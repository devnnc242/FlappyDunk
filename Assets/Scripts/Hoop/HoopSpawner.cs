using UnityEngine;

public class HoopSpawner : MonoBehaviour
{
    [SerializeField] private float spawnX = 7f;
    [SerializeField] private float maxY = 3f;
    [SerializeField] private float minY = -2f;

    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private int initialSpawnCount = 1;

    private float _nextSpawnX;

    private void OnEnable()
    {
        HoopBase.OnHoopPassed += HandlePassed;
        HoopBase.OnHoopMissed += HandleMissed;
    }

    private void OnDisable()
    {
        HoopBase.OnHoopPassed -= HandlePassed;
        HoopBase.OnHoopMissed -= HandleMissed;
    }

    private void Start()
    {
        _nextSpawnX = spawnX;

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnHoop();
        }
    }

    private void HandlePassed(IHoop hoop)
    {
        SpawnHoop();
    }

    private void HandleMissed(IHoop hoop)
    {
        GameManager.Ins.GameOver("Missed hoop!");
    }

    private void SpawnHoop()
    {
        HoopType type = GetRandomType();

        HoopBase hoop = HoopPool.Ins.Get(type);

        if (hoop == null) return;

        float randomY = Random.Range(minY, maxY);

        hoop.transform.position = new Vector2(_nextSpawnX, randomY);

        hoop.ResetState();

        HoopConveyor.Ins.AddHoop(hoop);

        _nextSpawnX = spawnDistance;
    }

    private HoopType GetRandomType()
    {
        float roll = Random.value;

        if (roll < 0.6f) return HoopType.Normal;

        if (roll < 0.9f) return HoopType.Moving;

        return HoopType.Spike;
    }
}