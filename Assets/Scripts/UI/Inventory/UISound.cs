using Recalled.Core;
using Recalled.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(RequestClipDispatcher))]
    [RequireComponent(typeof(Selectable))]
    internal sealed class UISound : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] SoundCue _clickSound;

        RequestClipDispatcher _clipDispatcher;

        public void OnPointerClick(PointerEventData eventData) => _clipDispatcher.Dispatch(_clickSound);

        private void Awake()
        {
            _clipDispatcher = GetComponent<RequestClipDispatcher>();
        }
    }
}
