using UnityEngine;

[RequireComponent(typeof(Chest))]
public sealed class ChestApproachPopup : ApproachTextPopup
{
    Chest _chest;

    private void Awake()
        => _chest = GetComponent<Chest>();
    private void OnEnable()
        => _chest.OnInteract += StopOnInteracted;
    private void OnDisable()
        => _chest.OnInteract -= StopOnInteracted;

    void StopOnInteracted()
    {
        enabled = false;
        Destroy(_behaviour.gameObject);
    }

    public override void Show()
    {
        if (_chest.PlayerCanInteract())
            base.Show();
    }
}
