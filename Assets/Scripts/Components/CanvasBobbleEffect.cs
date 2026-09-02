using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(UIBehaviour))]
public class CanvasBobbleEffect : MonoBehaviour
{
    [SerializeField] float _frequency = 5.5f;
    [SerializeField] float _amplitude = 0.07f;
    [SerializeField] DirectionEnum _direction;

    enum DirectionEnum { Vertical, Horizontal }

    Vector2 _basePosition;
    Transform _transform;
    UIBehaviour _uiElement;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _uiElement = GetComponent<UIBehaviour>();

        _basePosition = _transform.position;
    }

    void Update()
    {
        if (!_uiElement.enabled)
            return;

        float offset = Mathf.Sin(Time.time * _frequency) * _amplitude;
        if (_direction == DirectionEnum.Vertical)
            _transform.position = new(_basePosition.x, _basePosition.y + offset);
        else if (_direction == DirectionEnum.Horizontal)
            _transform.position = new(_basePosition.x + offset, _basePosition.y);
    }
}
