using UnityEngine;
using static Recalled.Gameplay.Chest.States;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Chest))]
    public class ChestAnimation : MonoBehaviour, IInteractionReactor
    {
        [SerializeField] Animator _animator;
        Chest _chest;

        private void Awake()
        {
            _chest = GetComponent<Chest>();
        }

        public void ReactToInteraction(Interactable _)
        {
            if (_chest.State == Opened)
            {
                _animator.SetTrigger(AnimatorParameters.InteractHash);
                enabled = false;
            }
        }
    }
}
