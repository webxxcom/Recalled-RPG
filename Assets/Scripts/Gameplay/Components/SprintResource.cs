using UnityEngine;

[DisallowMultipleComponent]
public class SprintingResource : ValueResource
{
    [SerializeField] int _usage;
    [SerializeField] int _restore;
    [SerializeField] float _speedMultiplier;
    [SerializeField] MovementBase _movementBase;

    public float SpeedMultiplier => _speedMultiplier;

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
