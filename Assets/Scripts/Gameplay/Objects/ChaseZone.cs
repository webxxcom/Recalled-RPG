using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaseZone : MonoBehaviour
{
    [SerializeField] LayerMask _trackedLayers;
    public GameObject CurrentTarget => _targets.FirstOrDefault();

    [Header("Broadcasts to")]
    [SerializeField] GameobjectGameEvent OnTargetEnteredTheZone;
    [SerializeField] GameobjectGameEvent OnTargetLeftTheZone;

    readonly List<GameObject> _targets = new(10);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_trackedLayers.value & (1 << collision.gameObject.layer)) != 0)
        {
            _targets.Add(collision.gameObject);

            OnTargetEnteredTheZone.Invoke(CurrentTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_targets.Remove(collision.gameObject))
        {
            OnTargetLeftTheZone.Invoke(CurrentTarget);
        }
    }
}
