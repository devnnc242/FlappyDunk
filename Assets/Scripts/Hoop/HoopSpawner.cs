using UnityEngine;

public class HoopSpawner : MonoBehaviour
{

    [SerializeField] private HoopPool hoopPool;

    [Header("Spawn")]
    [SerializeField] private float spawnX = 7f;

    [SerializeField] private float maxY = 3f;
    [SerializeField] private float minY = -2f;

    [SerializeField] private float spawnDistance = 5f;
    [SerializeField] private int initialSpawnCount = 1;

    private float _nextSpawnX;

    private void OnEnable()
    {
        HoopMover.OnHoopPassed += HandleHoopPassed;
        HoopMover.OnHoopMissed += HandleHoopMissed;
    }

    private void OnDisable()
    {
        HoopMover.OnHoopPassed -= HandleHoopPassed;
        HoopMover.OnHoopMissed -= HandleHoopMissed;
    }

    private void Start()
    {
        _nextSpawnX = spawnX;

        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnHoop();
        }
    }

    private void HandleHoopPassed(HoopMover mover)
    {
        SpawnHoop();
    }

    private void HandleHoopMissed(HoopMover mover)
    {
        GameManager.Ins.GameOver("Missed hoop!!");
    }

    public void SpawnHoop()
    {
        if (hoopPool == null) return;

        GameObject obj = hoopPool.GetHoop();

        float randomY = Random.Range(minY, maxY);

        obj.transform.position = new Vector2(_nextSpawnX, randomY);

        HoopMover mover = obj.GetComponent<HoopMover>();

        if (mover == null) return;
        mover.ResetState();

        obj.SetActive(true);

        _nextSpawnX = spawnDistance;
    }
}
