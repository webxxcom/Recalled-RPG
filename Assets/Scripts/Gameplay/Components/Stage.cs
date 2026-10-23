using Recalled.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Zone))]
    public sealed class Stage : MonoBehaviour
    {
        [SerializeField] UnityEvent _started;
        [SerializeField] UnityEvent _finished;
        [SerializeField] BoolRuntimeVariable _stageLevelVariable;

        IEnumerable<Door> _doors;
        IEnumerable<Health> _enemies;
        Zone _zone;
        bool _isStarted;

        private void Awake()
        {
            _enemies = GetComponentsInChildren<Health>().Where(h => h.CompareTag("Enemy"));
            _doors = GetComponentsInChildren<Door>();
            _zone = GetComponent<Zone>();
        }

        private void Start() => ApplyLevels();
        private void OnEnable() => _zone.Entered += CheckStageStart;
        private void OnDisable() => _zone.Entered -= CheckStageStart;

        void CheckStageStart(GameObject game)
        {
            if (_isStarted)
                return;

            if (game.CompareTag("Player"))
                StartStage();
        }

        void StartStage()
        {
            foreach (var door in _doors)
                door.SetState(Door.States.Closed);

            _isStarted = true;
            _started.Invoke();
        }

        void FinishStage()
        {
            _stageLevelVariable.Value = true;

            ApplyLevels();
            _finished.Invoke();
        }

        void ApplyLevels()
        {
            bool cleared = _stageLevelVariable.Value;

            if (cleared)
                foreach (var door in _doors)
                    door.SetState(Door.States.Open);
            enabled = !cleared;
        }

        private void Update()
        {
            foreach (var enemy in _enemies)
            {
                if (!enemy.IsDead)
                    return;
            }

            // All enemies are dead -> finish stage
            FinishStage();
        }
    }
}
