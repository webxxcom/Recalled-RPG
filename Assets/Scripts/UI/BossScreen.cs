using System;
using TMPro;
using UnityEngine;

public class BossHpUiManager : UiView
{
    [SerializeField] TextMeshProUGUI _bossText;
    [SerializeField] BarScriptUI _fillHpBar;

    [Header("Listens to")]
    [SerializeField] BossStartDataGameEvent OnBossStarted;
    [SerializeField] BossStartDataGameEvent OnBossDefeat;

    protected override void OnEnable()
    {
        base.OnEnable();
        OnBossStarted.OnEventRaised += StartBoss;
        OnBossDefeat.OnEventRaised += EndBoss;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnBossStarted.OnEventRaised -= StartBoss;
        OnBossDefeat.OnEventRaised -= EndBoss;
    }

    void StartBoss(BossData bossStartData)
    {
        // TODO resolve this boss appear
        //IsActive = true;

        _fillHpBar.Init(bossStartData.Health, bossStartData.Health.Value);
        _bossText.text = bossStartData.Name;
    }

    void EndBoss(BossData bossStartData)
    {
        //IsActive = false;
    }
}
