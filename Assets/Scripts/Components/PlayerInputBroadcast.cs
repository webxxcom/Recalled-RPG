using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInputBroadcast : MonoBehaviour
{
    [Header("Broadcasts to")]
    [SerializeField] VoidGameEvent OnInventory;
    [SerializeField] VoidGameEvent OnPauseMenu;

    void OnPause()
        => OnPauseMenu.Invoke();
    void OnToggleInventory(InputValue _)
        => OnInventory.Invoke();
}
