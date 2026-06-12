using System.Collections.Generic;
using UnityEngine;

public class HoopConveyor : Singleton<HoopConveyor>
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float limitX = -7f;

    public float MoveSpeed => moveSpeed;

    private readonly List<IHoop> _active = new();
    private readonly List<IHoop> _toRemove = new(); //avoid mid-loop mutation
    private bool _isPausedByRimContact;

    private void OnEnable()
    {
        DunkController.OnRimContactChanged += HandleRimContactChanged;
    }

    private void OnDisable()
    {
        DunkController.OnRimContactChanged -= HandleRimContactChanged;
        _isPausedByRimContact = false;
    }

    //Registration API
    public void Register(IHoop hoop)
    {
        if (!_active.Contains(hoop)) _active.Add(hoop);
    }

    public void Unregister(IHoop hoop) => _active.Remove(hoop);

    private void Update()
    {
        if (!GameManager.Ins.IsPlaying) return;
        if (_isPausedByRimContact) return;

        float delta = moveSpeed * Time.deltaTime;

        _toRemove.Clear();

        foreach (IHoop hoop in _active)
        {
            Transform t = hoop.Hoop.transform;
            t.Translate(Vector2.left * delta);

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

    private void HandleRimContactChanged(bool isTouching)
    {
        _isPausedByRimContact = isTouching;
    }
}
