using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    public class ToggleableGameobjectDisabler : MonoBehaviour
    {
        Toggleable _toggleable;
        Canvas _canvas;

        private void Awake()
        {
            _toggleable = GetComponent<Toggleable>();
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            _toggleable.StateChanged += OnToggled;
        }
        void OnDisable()
        {
            _toggleable.StateChanged -= OnToggled;
        }
        void OnToggled(bool isActived)
        {
            _canvas.enabled = isActived;
            _toggleable.gameObject.SetActive(isActived);
        }
    }
}
