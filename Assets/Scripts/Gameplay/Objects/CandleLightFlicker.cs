using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Light2D))]
    sealed class CandleLightFlicker : MonoBehaviour
    {
        Light2D _light2D;
        float _baseIntensity;
        float _baseRadius;

        private void Awake() => _light2D = GetComponent<Light2D>();

        private void Start()
        {
            _baseIntensity = _light2D.intensity;
            _baseRadius = _light2D.pointLightOuterRadius;
        }

        private void Update()
        {
            float slow = Mathf.PerlinNoise1D(Time.time * 0.5f) * 0.7f;
            float fast = Mathf.PerlinNoise1D(Time.time * 8f) * 0.3f;
            float noise = slow + fast; // roughly 0–1, weighted toward slow

            _light2D.intensity = _baseIntensity * Mathf.Lerp(0.3f, 1.0f, noise);
            _light2D.pointLightOuterRadius = _baseRadius * Mathf.Lerp(0.95f, 1.05f, noise);
        }
    }
}
