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
            => _chest.Interacted += StopOnInteracted;
        private void OnDisable()
            => _chest.Interacted -= StopOnInteracted;

        void StopOnInteracted()
        {
            enabled = false;
            Destroy(_behaviour.gameObject);
        }

        public override void Show()
        {
            if (_chest.CanBeInteracted)
                base.Show();
        }
    }
}
