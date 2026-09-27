using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StageController : MonoBehaviour
{
    [SerializeField] DoorsRuntimeSet _doors;
    [SerializeField] TransformRuntimeSet _enemies;
    [SerializeField] Collider2D _detectionZone;
    [SerializeField] StageClearenceLight _clearenceLight;

    [Header("Broadcasts to")]
    public VoidGameEvent OnStageStarted;
    public VoidGameEvent OnStageCleared;

    Collider2D _collider2D;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }
    private void OnEnable()
    {
        _enemies.OnChanged += CheckStageClear;
    }
    private void OnDisable()
    {
        _enemies.OnChanged -= CheckStageClear;
    }

    void CheckStageClear()
    {
        if (_enemies.Items.Count == 0)
        {
            foreach (var door in _doors.Items)
                door.Open();

            _clearenceLight.Activate();
            OnStageCleared.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collider2D.enabled = false;
            foreach (var door in _doors.Items)
                door.Close();
            
            OnStageStarted.Invoke();
        }
    }
}
