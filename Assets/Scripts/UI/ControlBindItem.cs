using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlBindItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _header;
    [SerializeField] TextMeshProUGUI _bind;
    [SerializeField] Button _rebind;

    InputAction _inputAction;

    private void OnEnable()
    {
        _rebind.onClick.AddListener(Rebind);
    }

    public void Init(InputAction inputAction)
    {
        _inputAction = inputAction;

        _bind.text = inputAction.bindings[0].isComposite
            ? inputAction.bindings[0].name
            : inputAction.bindings[0].ToDisplayString();
        _header.text = inputAction.name;
    }

    void Rebind()
    {

    }
}
