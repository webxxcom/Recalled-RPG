using System;

namespace Recalled.UI
{
    public interface IToggleable
    {
        event Action<bool> StateChanged;

        bool IsActive { get; }
        void SetIsActive(bool value);
    }
}
