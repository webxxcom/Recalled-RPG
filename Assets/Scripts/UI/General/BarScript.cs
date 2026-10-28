using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Recalled.UI
{
    public sealed class BarScript : MonoBehaviour
    {
        [SerializeField] Image _topBar;
        [SerializeField] Image _bottomBar;
        [SerializeField] float _animationSpeed = 1f;

        float _currentValue;

        public UnityEvent ProgressStarted;
        public UnityEvent ProgressEnded;

        void Start() => Set(1);

        IEnumerator ProgressBars(float delta)
        {
            var suddenBar = delta <= 0 ? _topBar : _bottomBar;
            var smoothBar = delta <= 0 ? _bottomBar : _topBar;

            ProgressStarted.Invoke();
            suddenBar.fillAmount = _currentValue;
            while (Mathf.Abs(_currentValue - smoothBar.fillAmount) > 0.01f)
            {
                smoothBar.fillAmount
                    = Mathf.Lerp(smoothBar.fillAmount, _currentValue, Time.deltaTime * _animationSpeed);
                yield return null;
            }
            smoothBar.fillAmount = _currentValue;
            ProgressEnded.Invoke();
        }

        public void Set(float ratio)
        {
            float prevValue = _currentValue;
            _currentValue = ratio;

            StopAllCoroutines();
            StartCoroutine(ProgressBars(_currentValue - prevValue));
        }
    }
}