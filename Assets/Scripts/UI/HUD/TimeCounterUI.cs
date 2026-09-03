using System;
using TMPro;
using UnityEngine;

public sealed class TimeCounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timeCounter;

    float _elapsed = 0;
    private void Update()
    {
        _elapsed += Time.deltaTime;

        _timeCounter.text = TimeSpan.FromSeconds(_elapsed).ToString(@"mm\:ss\.ff");
    }
}
