using UnityEngine;
using UnityEngine.Events;

namespace Recalled.UI
{
    public interface IToggleable
    {
        GameObject gameObject { get; }
        UnityEvent<bool> StateChanged { get; }
        bool IsActive { get; }
        void SetIsActive(bool value);
    }
}
