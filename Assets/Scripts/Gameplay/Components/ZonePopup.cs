using UnityEngine;

namespace Recalled.Gameplay
{
    public sealed class ZonePopup : MonoBehaviour, IZoneReactor
    {
        [SerializeField] GameObject _popup;
        [SerializeField] Vector2 _offset;

        public bool IsLevel => true;

        GameObject _instance;

        private void Awake()
        {
            _instance = Instantiate(_popup, (Vector2)transform.position + _offset, Quaternion.identity, transform);
        }

        public void Present(Zone approachable)
        {
            SetIsShown(true);
        }

        public void Absent(Zone approachable)
        {
            SetIsShown(false);
        }

        public void SetIsShown(bool isShown)
        {
            _instance.SetActive(isShown);
        }
    }
}
