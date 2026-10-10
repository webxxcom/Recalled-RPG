using System.Collections;
using TMPro;
using UnityEngine;

namespace Recalled.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class PopupWorldText : MonoBehaviour
    {
        [SerializeField] float _duration;
        [SerializeField] Vector2 _direction;
        [SerializeField] AnimationCurve _distanceOverTime;

        TMP_Text _tmpText;

        public void Init(string text)
        {
            _tmpText.text = text;
        }

        void Awake()
        {
            _tmpText = GetComponent<TMP_Text>();
        }

        void Start()
        {
            StartCoroutine(MovePosition());
        }

        private IEnumerator MovePosition()
        {
            Vector3 startPosition = transform.position;

            float elapsed = 0f;
            while (elapsed < _duration)
            {
                float t = Mathf.Clamp01(elapsed / _duration);

                float distance = _distanceOverTime.Evaluate(t);
                transform.position = startPosition + (Vector3)(_direction * distance);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
