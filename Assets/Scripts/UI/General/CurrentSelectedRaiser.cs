using UnityEngine;
using UnityEngine.EventSystems;
using Recalled.Core;

namespace Recalled.UI
{
    [RequireComponent(typeof(EventSystem))]
    sealed class CurrentSelectedRaiser : MonoBehaviour
    {
        [Header("Sets Reference to")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        GameObject _selectedObject;
        EventSystem _eventSystem;

        private void Awake() => _eventSystem = GetComponent<EventSystem>();

        private void Update()
        {
            if (_selectedObject != _eventSystem.currentSelectedGameObject)
            {
                _selectedObject = _eventSystem.currentSelectedGameObject;
                _currentSelected.Value = _selectedObject;
            }
        }
    }
}
