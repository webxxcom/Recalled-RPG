using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerMovement))]
public class SprintingResource : ValueResource
{
    [SerializeField] int _usage;
    [SerializeField] int _restore;
    [SerializeField] float _speedMultiplier;

    public float SpeedMultiplier => _speedMultiplier;

    MovementBase _movementBase;
    bool _isActive;
    
    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                if (value)
                    _movementBase.AddSpeedCoef(_speedMultiplier);
                else
                    _movementBase.RemoveSpeedCoef(_speedMultiplier);

                _isActive = value;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _movementBase = GetComponent<PlayerMovement>();
    }

    public void Toggle(bool isActive)
        => IsActive = isActive;

    private void FixedUpdate()
    {
        if (IsActive)
        {
            if (Consume(_usage) == 0)
                IsActive = false;
        }
        else Replenish(_restore);
    }
}
