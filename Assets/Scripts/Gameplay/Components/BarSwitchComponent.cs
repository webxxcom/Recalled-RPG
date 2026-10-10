using UnityEngine;

public class BarSwitchComponent : MonoBehaviour
{
    [SerializeField] GameObject _aliveBar;
    [SerializeField] GameObject _deadBar;
    [SerializeField] ValueResource _healthProvider;

    void ToggleBars(int _)
    {
        _aliveBar.SetActive(!_aliveBar.activeInHierarchy);
        _deadBar.SetActive(!_deadBar.activeInHierarchy);
    }

    private void OnEnable()
    {
        _healthProvider.MinValueReached += ToggleBars;
    }

    private void OnDisable()
    {
        _healthProvider.MinValueReached -= ToggleBars;
    }
}
