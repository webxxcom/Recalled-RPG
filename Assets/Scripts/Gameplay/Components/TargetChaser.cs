using UnityEngine;

namespace Recalled.Gameplay
{
    public class TargetChaser : MonoBehaviour, ITargetProvider
    {
        [SerializeField] EyeSight _eyeSight;
        [SerializeField] float _losesInterestAfter = 2f;

        public GameObject Target { get; private set; }
        public bool HasTarget => Target != null;

        private void OnEnable()
        {
            _eyeSight.SawSomeone += UpdateTarget;
        }
        private void OnDisable()
        {
            _eyeSight.SawSomeone -= UpdateTarget;
        }

        float _elapsedLastSet;
        void UpdateTarget(GameObject target)
        {
            if (Target != null) // have target -> don't change
                return;

            _elapsedLastSet = 0;
            Target = target;
        }

        private void Update()
        {
            if (Target != null && !_eyeSight.IsWithinSight(Target))
                _elapsedLastSet += Time.deltaTime;

            if (_elapsedLastSet >= _losesInterestAfter)
                Target = null;
        }
    }
}
