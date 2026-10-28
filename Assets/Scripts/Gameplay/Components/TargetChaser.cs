using UnityEngine;

namespace Recalled.Gameplay
{
    public class TargetChaser : MonoBehaviour, IChaser, ITargetProvider
    {
        [SerializeField] float _losesInterestAfter = 2f;
        [SerializeField] float _minDistance = 1f;
        EyeSight _eyeSight;

        public GameObject Target { get; private set; }
        public bool HasTarget => Target != null;
        public Vector2 Direction
        {
            get
            {
                if (!HasTarget)
                    return Vector2.zero;

                var dist = Target.transform.position - transform.position;
                if (_minDistance * _minDistance >= dist.sqrMagnitude)
                    return Vector2.zero;

                return dist.normalized;
            }
        }
        private void Awake() => _eyeSight = GetComponentInChildren<EyeSight>();
        private void OnEnable() => _eyeSight.SawSomeone += UpdateTarget;
        private void OnDisable() => _eyeSight.SawSomeone -= UpdateTarget;

        float _elapsedLastSet;
        void UpdateTarget(GameObject target)
        {
            if (Target != null || !target.CompareTag("Player")) // have target -> don't change
                return;

            _elapsedLastSet = 0;
            Target = target;
        }

        private void Update()
        {
            if (Target != null && !_eyeSight.IsWithinSight(Target))
                _elapsedLastSet += Time.deltaTime;

            if (_elapsedLastSet >= _losesInterestAfter || (Target != null && Target.TryGetComponent<Health>(out var health) && health.IsDead))
                Target = null;
        }
    }
}
