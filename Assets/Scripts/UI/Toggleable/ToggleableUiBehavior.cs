using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    public class ToggleableUiBehavior : MonoBehaviour
    {
        [SerializeField] GameObject _firstSelected;

        Toggleable _toggleable;
        Canvas _canvas;

        private void Awake()
        {
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();

            _toggleable.StateChanged.AddListener(OnToggled);
        }
        private void Start() => OnToggled(_toggleable.IsActive);

        void OnDestroy()
        {
            _toggleable.StateChanged.RemoveListener(OnToggled);
        }
        void OnToggled(bool isActived)
        {
            _canvas.enabled = isActived;
            if (_firstSelected && isActived)
            {
                var eventSystem = EventSystem.current;
                if (eventSystem == null)
                    return;

                var current = eventSystem.currentSelectedGameObject;
                if (current != null && !current.transform.IsChildOf(transform)) // Do not override current selected
                    EventSystem.current.SetSelectedGameObject(_firstSelected);
            }
        }
    }
}
