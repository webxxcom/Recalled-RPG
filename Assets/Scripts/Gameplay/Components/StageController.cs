using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] TransformRuntimeSet _enemies;
        [SerializeField] Collider2D _detectionZone;

        [Header("Sets")]
        [SerializeField] VoidGameEvent _stageStarted;
        [SerializeField] BoolRuntimeVariable _stageIsCleared;

        private void OnEnable()
        {
            _enemies.Changed += CheckStageClear;
        }
        private void OnDisable()
        {
            _enemies.Changed -= CheckStageClear;
        }

        void CheckStageClear()
        {
            if (_enemies.Items.Count == 0)
                _stageIsCleared.Value = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _detectionZone.enabled = false;

                _stageStarted.Invoke();
            }
        }
    }
}
