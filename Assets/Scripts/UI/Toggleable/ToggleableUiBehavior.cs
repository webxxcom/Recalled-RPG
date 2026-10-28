using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(Toggleable))]
    [RequireComponent(typeof(Canvas))]
    public class ToggleableUiBehavior : MonoBehaviour
    {
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
        }
    }
}
