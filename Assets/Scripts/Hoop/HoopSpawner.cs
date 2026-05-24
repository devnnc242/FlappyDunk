using UnityEngine;

public class HoopSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HoopPool hoopPool;

    [Header("Spawner setting")]
    [SerializeField] private float spawnRate = 2f;

    [SerializeField] private float spawnX = 10f;

    [Header("Height Range")]
    [SerializeField] private float minY = -2f;

    [SerializeField] private float maxY = 3f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= spawnRate)
        {
            _timer = 0f;

            SpawnHoop();
        }
    }

    private void SpawnHoop()
    {
        GameObject hoop = hoopPool.GetHoop();

        float randomY = Random.Range(minY, maxY);

        hoop.transform.position = new Vector3(spawnX, randomY, 0f);

        hoop.SetActive(true);
    }
}
