using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class BarSwitcher : MonoBehaviour
    {
        [SerializeField] GameObject _aliveBar;
        [SerializeField] GameObject _deadBar;

        public void SwitchBars()
        {
            _aliveBar.SetActive(!_aliveBar.activeInHierarchy);
            _deadBar.SetActive(!_deadBar.activeInHierarchy);
        }
    }
}
