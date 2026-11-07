using Recalled.Gameplay;
using Recalled.UI.Inventory;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Recalled.UI
{
    public class UseItem : MonoBehaviour
    {
        [SerializeField] InputActionReference _useAction;
        [SerializeField] ItemProcessorAnchor _itemProcessorAnchor;

        void OnEnable() => _useAction.action.performed += OnUseAction;
        void OnDisable() => _useAction.action.performed -= OnUseAction;
        void OnUseAction(InputAction.CallbackContext _) => Use();

        void Use()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current == null || !current.TryGetComponent<IItemHolder>(out var slot))
                return;

            if (_itemProcessorAnchor.IsSet)
                _itemProcessorAnchor.Value.TryToggleEquip(slot.Item);
            else Debug.LogWarning($"The {nameof(_itemProcessorAnchor)}'s value was not provided");
        }
    }
}
