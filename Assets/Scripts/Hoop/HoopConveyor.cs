using System.Collections.Generic;
using UnityEngine;

public class HoopConveyor : Singleton<HoopConveyor>
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float limitX = -7f;

    private readonly HashSet<IHoop> _active = new();
    private readonly List<IHoop> _toRemove = new(); //avoid mid-loop mutation

    private GameManager _gameManager;

    public float MoveSpeed => moveSpeed;

    protected override void Awake()
    {
        base.Awake();

        _gameManager = GameManager.Ins;
    }

    //Registration API
    public void AddHoop(IHoop hoop)
    {
        if (hoop == null) return;

        _active.Add(hoop);
    }

    public void RemoveHoop(IHoop hoop)
    {
        if (hoop == null) return;

        _active.Remove(hoop);
    }

    private void Update()
    {
        if (!_gameManager.IsPlaying) return;

        float delta = moveSpeed * Time.deltaTime;

        _toRemove.Clear();

        foreach (IHoop hoop in _active)
        {
            Transform t = hoop.Hoop.transform;

            t.Translate(Vector3.left * delta, Space.World);

            if (t.position.x < limitX)
            {
                _toRemove.Add(hoop);

                hoop.OnReadchedLimit();
            }
        }

        foreach (IHoop hoop in _toRemove)
        {
            _active.Remove(hoop);
        }
    }

    public void SetSpeed(float speed) => moveSpeed = speed;
}
