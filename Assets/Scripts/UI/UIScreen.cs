using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public abstract class UIScreen : MonoBehaviour
{
    Canvas _canvas;

    public bool IsActive
    {
        get => gameObject.activeInHierarchy;
        set
        {
            if (value == IsActive)
                return;

            gameObject.SetActive(value);

            if (value) Open();
            else Close();

            OnStateChange?.Invoke(this, value);
        }
    }

    public event Action<UIScreen, bool> OnStateChange;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    protected virtual void Start()
    {
        _canvas.enabled = true;
        IsActive = false;
    }

    protected void Toggle() => IsActive = !IsActive;

    public virtual void Open() { }
    public virtual void Close() { }
}
