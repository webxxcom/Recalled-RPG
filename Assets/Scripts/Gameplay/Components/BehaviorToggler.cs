using UnityEngine;

public class BehaviorToggler : MonoBehaviour
{
    [SerializeField] protected Behaviour _behaviour;

    private void Start()
    {
        _behaviour.enabled = false;
    }

    public virtual void Show()
    {
        _behaviour.enabled = true;
    }

    public virtual void Hide()
    {
        _behaviour.enabled = false;
    }
}
