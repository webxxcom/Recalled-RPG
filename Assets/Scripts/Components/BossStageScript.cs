using UnityEngine;

[RequireComponent(typeof(StageScript))]
public class BossStageScript : MonoBehaviour
{
    [SerializeField] BossDataVariable _bossData;

    [Header("Broadcasts to")]
    [SerializeField] BossStartDataGameEvent OnBossStart;
    [SerializeField] BossStartDataGameEvent OnBossDefeat;

    StageScript _stageScript;

    private void Awake()
        => _stageScript = GetComponent<StageScript>();
    private void OnEnable()
    {
        _stageScript.OnStageStarted.AddListener(BossStart);
        _stageScript.OnStageCleared.AddListener(BossDefeat);
    }
    private void OnDisable()
    {
        _stageScript.OnStageStarted.RemoveListener(BossStart);
        _stageScript.OnStageCleared.RemoveListener(BossDefeat);
    }

    void BossStart()
    {
        OnBossStart.Invoke(_bossData.Value);
    }

    void BossDefeat()
    {
        OnBossDefeat.Invoke(_bossData.Value);
    }
}
