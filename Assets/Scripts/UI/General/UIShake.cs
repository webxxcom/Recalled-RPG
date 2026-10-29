using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    /// <summary>
    /// Short damped shake for UI feedback, such as a rejected action.
    /// Put it on a visual child, not on an object positioned by a layout group:
    /// the layout group owns its direct children's positions and would fight the shake.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIShake : MonoBehaviour
    {
        [Tooltip("Direction of the shake. (1, 0) is a horizontal 'no' shake.")]
        [SerializeField] Vector2 _direction = Vector2.right;

        [Tooltip("Largest offset from rest, in canvas units.")]
        [SerializeField, Min(0f)] float _amplitude = 2f;

        [Tooltip("Back-and-forth movements per second.")]
        [SerializeField, Min(0f)] float _frequency = 18f;

        [Tooltip("Total duration in seconds. The amplitude fades to zero over this time.")]
        [SerializeField, Min(0.01f)] float _duration = 0.25f;

        [Tooltip("Round the offset to whole canvas units, so pixel art never lands between pixels. " +
                 "This matches screen pixels only if one canvas unit is one art pixel on your canvas.")]
        [SerializeField] bool _wholeUnits = true;

        RectTransform _rectTransform;
        Vector2 _rest;
        float _elapsed;
        bool _isPlaying;

        void Awake()
        {
            _rectTransform = (RectTransform)transform;
            WarnIfDrivenByLayout();
        }

        // A hidden screen must not keep a leftover offset.
        void OnDisable() => Stop();

        /// <summary>Starts the shake, or restarts it from rest if it's already playing.</summary>
        public void Play()
        {
            // An inactive object doesn't update; starting now would play the shake late, when it reappears.
            if (!isActiveAndEnabled)
                return;

            // Capture rest only when idle. Mid-shake, the current position includes an offset.
            if (!_isPlaying)
                _rest = _rectTransform.anchoredPosition;

            _elapsed = 0f;
            _isPlaying = true;
        }

        /// <summary>Stops immediately and returns to rest.</summary>
        public void Stop()
        {
            if (!_isPlaying)
                return;

            _isPlaying = false;
            _rectTransform.anchoredPosition = _rest;
        }

        void Update()
        {
            if (!_isPlaying)
                return;

            // Unscaled time keeps the shake playing in menus that pause the game with Time.timeScale = 0.
            _elapsed += Time.unscaledDeltaTime;
            if (_elapsed >= _duration)
            {
                Stop();
                return;
            }

            // Damped sine: full amplitude at the start, fading linearly to zero at the end.
            // Always rest + offset, never current + offset, so repeated shakes can't drift.
            float fade = 1f - _elapsed / _duration;
            float wave = Mathf.Sin(2f * Mathf.PI * _frequency * _elapsed);
            Vector2 offset = _direction.normalized * (_amplitude * fade * wave);

            if (_wholeUnits)
                offset = new Vector2(Mathf.Round(offset.x), Mathf.Round(offset.y));

            _rectTransform.anchoredPosition = _rest + offset;
        }

        void WarnIfDrivenByLayout()
        {
            Transform parent = transform.parent;
            if (parent == null || !parent.TryGetComponent(out LayoutGroup _))
                return;

            if (TryGetComponent(out LayoutElement element) && element.ignoreLayout)
                return;

            Debug.LogWarning($"{name}: {nameof(UIShake)} is on a child of a layout group, which owns this " +
                             "object's position and will fight the shake. Put it on a visual child instead.", this);
        }
    }
}