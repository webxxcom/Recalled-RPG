using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    public sealed class ToggleableSelectFirst : MonoBehaviour
    {
        [SerializeField] Selectable _selectFirst;

        Toggleable _toggleable;

        void Awake() => _toggleable = GetComponent<Toggleable>();

        void OnEnable()
        {
            _toggleable.StateChanged += OnToggled;
        }

        void OnDisable()
        {
            _toggleable.StateChanged -= OnToggled;
        }

        void OnToggled(bool flag)
        {
            if (_selectFirst != null)
                EventSystem.current.SetSelectedGameObject(_selectFirst.gameObject);
            else
            {
                //var selectable = GetComponentInChildren<Selectable>(true);
                //if (selectable != null)
                //    EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            }
        }
    }
}
