using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(EyeSight))]
    internal sealed class InteractionProcessor : MonoBehaviour
    {
        [SerializeField] bool _drawGizmos;

        EyeSight _eyeSight;

        private void Awake() => _eyeSight = GetComponent<EyeSight>();

        void OnInteract(InputValue _)
        {
            var closest = _eyeSight.FindClosest();
            if (closest != null)
                closest.GetComponent<Interactable>().Interact(this);
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos || !_eyeSight)
                return;

            var closest = _eyeSight.FindClosest();
            if (closest != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, closest.transform.position);
            }
        }
    }
}
