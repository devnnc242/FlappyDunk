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

    //Handlers
    private void HandleHoopPassed(IHoop hoop) => SpawnHoop();
    private void HandleHoopMissed(IHoop hoop) => GameManager.Ins.GameOver("Missed hoop!");

    public void SpawnHoop()
    {
        if (hoopPool == null) return;

        GameObject obj = hoopPool.GetHoop();
        if (obj == null) return;

        float randomY = Random.Range(minY, maxY);

        obj.transform.position = new Vector2(_nextSpawnX, randomY);

        IHoop hoop = obj.GetComponent<IHoop>();

        if (hoop == null) return;

        hoop.ResetState();

        obj.SetActive(true);
        HoopConveyor.Ins.Register(hoop);

        _nextSpawnX = spawnDistance;
    }
}
