using UnityEngine;

public abstract class AutoSetRegistor<T> : MonoBehaviour
{
    [SerializeField] RuntimeSet<T> _gameSet;

    protected abstract T Data { get; }

    private void OnEnable() => _gameSet.Add(Data);
    private void OnDisable() => _gameSet.Remove(Data);
}
