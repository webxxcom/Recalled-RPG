using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class PopupWorldText : MonoBehaviour
{
    [SerializeField] float _duration;
    [SerializeField] Vector2 _direction;
    [SerializeField] ParticleSystem.MinMaxCurve _distanceOverTime;

    TextMeshPro _textMeshPro;

    public void Init(string text)
    {
        _textMeshPro.text = text;
    }

    void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _distanceOverTime.curveMultiplier = 1;
    }

    void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 startPosition = transform.position;

        float elapsed = 0f;
        while (elapsed < _duration)
        {
            float t = Mathf.Clamp01(elapsed / _duration);

            float distance = _distanceOverTime.Evaluate(t);
            transform.position = startPosition + (Vector3)(_direction * distance);

            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
