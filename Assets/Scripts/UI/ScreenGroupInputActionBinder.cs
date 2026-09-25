using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ToggleableGroup))]
public class ScreenGroupInputActionBinder : MonoBehaviour
{
    [SerializeField] InputActionReference _prevPageAction;
    [SerializeField] InputActionReference _nextPageAction;

    ToggleableGroup _screenGroup;

    private void Awake()
    {
        _screenGroup = GetComponent<ToggleableGroup>();
    }

    private void OnEnable()
    {
        _nextPageAction.action.canceled += NextPage;
        _prevPageAction.action.canceled += PrevPage;
    }

    private void OnDisable()
    {
        _nextPageAction.action.canceled -= NextPage;
        _prevPageAction.action.canceled -= PrevPage;
    }

    void NextPage(InputAction.CallbackContext context)
    {
        _screenGroup.ChooseNext(1);
    }
    void PrevPage(InputAction.CallbackContext context)
    {
        _screenGroup.ChooseNext(-1);
    }
}
