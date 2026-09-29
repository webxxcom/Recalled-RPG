using UnityEngine;


namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Chest))]
    public sealed class ChestApproachPopup : BehaviorToggler
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
}
