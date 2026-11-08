using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    internal sealed class SelectOnAction : MonoBehaviour, ISubmitHandler, ICancelHandler
    {
        [SerializeField] Selectable _selected;
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

        void ProceedToSelected() => _selected.Select();
    }
}
