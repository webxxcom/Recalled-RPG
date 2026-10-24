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
        }
        private void Start() => OnToggled(_toggleable.IsActive);

        private void OnEnable()
        {
            _toggleable.StateChanged.AddListener(OnToggled);
        }

        void OnDisable()
        {
            _toggleable.StateChanged.RemoveListener(OnToggled);
        }
        void OnToggled(bool isActived)
        {
            _canvas.enabled = isActived;
            _toggleable.gameObject.SetActive(isActived);
        }
    }
}
