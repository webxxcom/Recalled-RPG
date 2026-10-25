using Recalled.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    [RequireComponent(typeof(UISpriteAnimator))]
    [RequireComponent(typeof(RectTransform))]
    public sealed class Highlighter : MonoBehaviour
    {
        [SerializeField] Image _image;
        [SerializeField] Transform _parentZone;

        [Header("Watches")]
        [SerializeField] GameObjectRuntimeVariable _currentSelected;

        UISpriteAnimator _animator;
        RectTransform _rectTransform;

        private void Awake()
        {
            _animator = GetComponent<UISpriteAnimator>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _currentSelected.Subscribe(ShowAt);
        }
        private void OnDisable()
        {
            _currentSelected.Unsubscribe(ShowAt);
        }

        void Show(GameObject game)
        {
            if (!game.transform.IsChildOf(_parentZone))
                return;

            _animator.Play("Idle");
            _image.enabled = true;
            _image.preserveAspect = true;
            transform.position = game.transform.position;
            transform.SetParent(game.transform);
            transform.SetAsLastSibling();

            var rect = game.GetComponent<RectTransform>().rect;
            _rectTransform.sizeDelta = new(rect.width + 2, rect.height + 2);
            _image.GetComponent<RectTransform>().sizeDelta = _rectTransform.sizeDelta;
        }

        void Hide()
        {
            _image.enabled = false;
            _animator.Stop();
        }

        public void ShowAt(GameObject game)
        {
            if (game == null) Hide();
            else Show(game);
        }
    }
}
