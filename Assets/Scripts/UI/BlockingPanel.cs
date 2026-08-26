using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockingPanel : MonoBehaviour
{
    [SerializeField] InputActionAsset _input;
    [SerializeField] TextMeshProUGUI _mainText;
    [SerializeField] float _timeout;

    private void Start()
    {
        _input.Disable();
    }

    float _elapsed;
    private void Update()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= _timeout || Keyboard.current.escapeKey.isPressed)
        {
            _input.Enable();
            Destroy(gameObject);
        }
    }
}
