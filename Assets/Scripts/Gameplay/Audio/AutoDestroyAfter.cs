using UnityEngine;

namespace Recalled.Gameplay
{
    sealed class AutoDestroyAfter : MonoBehaviour
    {
        [SerializeField] bool _isFixedTime;
        [SerializeField] float _timeToLive;

        public void Init(bool isFixedTime, float timeToLive)
        {
            _isFixedTime = isFixedTime;
            _timeToLive = timeToLive;
        }

        float _elapsed = 0;
        private void Update()
        {
            _elapsed += _isFixedTime ? Time.fixedDeltaTime : Time.deltaTime;

            if (_elapsed >= _timeToLive)
                Destroy(gameObject);
        }
    }
}
