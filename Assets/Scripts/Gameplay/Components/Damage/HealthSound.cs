using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay.Assets.Scripts.Gameplay.Components.Damage
{
    public class HealthSound : MonoBehaviour
    {
        [SerializeField] SoundCue _hurtSound;
        [SerializeField] SoundCue _healingSound;
        [SerializeField] SoundCue _deathSound;
        [SerializeField] Health _healthProvider;
        [SerializeField] AudioClipRequestGameEvent _audioClipRequestGameEvent;

        void OnEnable()
        {
            _healthProvider.Died += RequestDeathSound;
            _healthProvider.HpChanged += RequestHpChangedSound;
        }

        void OnDisable()
        {
            _healthProvider.Died -= RequestDeathSound;
            _healthProvider.HpChanged -= RequestHpChangedSound;
        }

        public void RequestDeathSound(DamageInfo _) => Invoke(_deathSound);
        public void RequestHpChangedSound(DamageInfo di)
        {
            if (di.Amount >= 0) Invoke(_hurtSound);
            else Invoke(_healingSound);
        }

        void Invoke(SoundCue cue)
        {
            _audioClipRequestGameEvent.Invoke(new(cue, transform.position));
        }
    }
}
