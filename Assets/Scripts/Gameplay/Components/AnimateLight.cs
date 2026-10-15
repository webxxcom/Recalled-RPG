using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Light2D))]
    public sealed class AnimateLight : MonoBehaviour
    {
        [Tooltip("Normalized intensity 0-1 where 0 is no light at all and 1 is base intensity")]
        [SerializeField] AnimationCurve _intensityOverTime;
        [SerializeField] float _duration;

        Light2D _light;
        float _baseIntensity;

        private void Awake()
        {
            _light = GetComponent<Light2D>();

            _baseIntensity = _light.intensity;
            _light.intensity = 0;
        }

        IEnumerator ProcessLightIntensityOverTime()
        {
            float elapsed = 0;
            while (elapsed < _duration)
            {
                _light.intensity = _baseIntensity * _intensityOverTime.Evaluate(elapsed);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        public void LightUp(bool yes)
        {
            if (yes)
            {
                StopAllCoroutines();
                StartCoroutine(ProcessLightIntensityOverTime());
            }
        }
    }
}
