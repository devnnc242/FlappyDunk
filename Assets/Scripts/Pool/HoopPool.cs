using UnityEngine;
using System.Collections.Generic;

public class HoopPool : MonoBehaviour
{
    [SerializeField] private GameObject hoopPrefab;
    [SerializeField] private int poolSize = 10;

    private readonly List<GameObject> _pool = new();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateHoop();
        }
    }

    private GameObject CreateHoop()
    {
        GameObject hoop = Instantiate(hoopPrefab, transform);

        hoop.SetActive(false);

        _pool.Add(hoop);

        return hoop;
    }

    public GameObject GetHoop()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }

        return CreateHoop();
    }
}
