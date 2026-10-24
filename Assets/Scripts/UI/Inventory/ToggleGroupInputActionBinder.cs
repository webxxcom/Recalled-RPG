using UnityEngine;
using UnityEngine.InputSystem;

namespace Recalled.UI.Inventory
{
    [RequireComponent(typeof(ToggleableGroup))]
    public class ToggleGroupInputActionBinder : MonoBehaviour
    {
        [SerializeField] InputActionReference _prevAction;
        [SerializeField] InputActionReference _nextAction;

        ToggleableGroup _group;

        private void Awake() =>  _group = GetComponent<ToggleableGroup>();
        private void OnEnable()
        {
            _nextAction.action.canceled += NextPage;
            _prevAction.action.canceled += PrevPage;
        }
        private void OnDisable()
        {
            _nextAction.action.canceled -= NextPage;
            _prevAction.action.canceled -= PrevPage;
        }

        void NextPage(InputAction.CallbackContext _) => _group.ChooseNext(1);
        void PrevPage(InputAction.CallbackContext _) => _group.ChooseNext(-1);
    }
}
