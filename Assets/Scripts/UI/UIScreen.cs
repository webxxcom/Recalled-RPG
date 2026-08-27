using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIScreen : MonoBehaviour
{
    Canvas _canvas;

    public bool IsActive
    {
        get => _canvas.enabled;
        set
        {
            if (value == IsActive)
                return;

            if (value) Open();
            else Close();

            OnStateChange?.Invoke(this, value);
            _canvas.enabled = value;
        }
    }

    public event Action<UIScreen, bool> OnStateChange;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    public virtual void Open() { }
    public virtual void Close() { }
}
