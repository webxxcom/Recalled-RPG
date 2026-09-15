using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ScreenGroup))]
public class ScreenGroupInputActionBinder : MonoBehaviour
{
    [SerializeField] InputActionReference _prevPageAction;
    [SerializeField] InputActionReference _nextPageAction;

    ScreenGroup _screenGroup;

    private void Awake()
    {
        _screenGroup = GetComponent<ScreenGroup>();
    }

    private void OnEnable()
    {
        _nextPageAction.action.started += NextPage;
        _prevPageAction.action.started += PrevPage;
    }

    private void OnDisable()
    {
        _nextPageAction.action.started -= NextPage;
        _prevPageAction.action.started -= PrevPage;
    }

    void NextPage(InputAction.CallbackContext _) => _screenGroup.ChooseNext(1);
    void PrevPage(InputAction.CallbackContext _) => _screenGroup.ChooseNext(-1);
}
