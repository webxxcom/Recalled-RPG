using UnityEngine;
using UnityEngine.EventSystems;

namespace Recalled.UI
{
    sealed class SelectOnAction : MonoBehaviour, ISubmitHandler, ICancelHandler
    {
        [SerializeField] GameObject _selected;
        [SerializeField] bool _onCancel;
        [SerializeField] bool _onSubmit;

        public void OnCancel(BaseEventData _)
        {
            if (_onCancel) ProceedToSelected();
        }
        public void OnSubmit(BaseEventData _)
        {
            if (_onSubmit) ProceedToSelected();
        }

        void ProceedToSelected()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null)
                return;

            var current = eventSystem.currentSelectedGameObject;
            if (current != null)
                eventSystem.SetSelectedGameObject(_selected);
        }
    }
}
