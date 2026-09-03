using TMPro;
using UnityEngine;

public class BossHpUiManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _bossText;
    [SerializeField] BarScriptUI _fillHpBar;
    [SerializeField] Canvas _canvas;

    [Header("Listens to")]
    [SerializeField] BossStartDataGameEvent OnBossStarted;
    [SerializeField] BossStartDataGameEvent OnBossDefeat;

    private void OnEnable()
    {
        OnBossStarted.OnEventRaised += StartBoss;
        OnBossDefeat.OnEventRaised += EndBoss;
    }
    private void OnDisable()
    {
        OnBossStarted.OnEventRaised -= StartBoss;
        OnBossDefeat.OnEventRaised -= EndBoss;
    }

    void StartBoss(BossData bossStartData)
    {
        _canvas.enabled = true;

        _fillHpBar.Init(bossStartData.Health, bossStartData.Health.Value);
        _bossText.text = bossStartData.Name;
    }

    void EndBoss(BossData bossStartData)
    {
        _canvas.enabled = false;
    }
}
