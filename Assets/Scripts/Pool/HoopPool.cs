using UnityEngine;
using System.Collections.Generic;

public class HoopPool : MonoBehaviour
{
    [SerializeField] private GameObject hoopPrefab;
    [SerializeField] private int initialSize = 4;

    private readonly List<GameObject> _pool = new();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateHoop();
        }
    }

    public GameObject GetHoop()
    {
        foreach (var obj in _pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }

        return CreateHoop();
    }

    // public void Return(GameObject hoop)
    // {
    //     hoop.SetActive(false);
    // }

    // public void ReturnAll()
    // {
    //     foreach (var obj in _pool)
    //     {
    //         obj.SetActive(false);
    //     }
    // }

    private GameObject CreateHoop()
    {
        GameObject obj = Instantiate(hoopPrefab, transform);

        obj.SetActive(false);

        _pool.Add(obj);

        return obj;
    }
}
