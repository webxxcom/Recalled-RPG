using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class UIEventRaiser : MonoBehaviour
{
    [Header("Sets Value to")]
    [SerializeField] GameObjectRuntimeVariable _currentSelected;

    GameObject _selectedObject;
    EventSystem _eventSystem;

    private void Awake()
        => _eventSystem = GetComponent<EventSystem>();

    private void Update()
    {
        if (_selectedObject != _eventSystem.currentSelectedGameObject)
        {
            _selectedObject = _eventSystem.currentSelectedGameObject;
            _currentSelected.Value = _selectedObject;
        }
    }
}
