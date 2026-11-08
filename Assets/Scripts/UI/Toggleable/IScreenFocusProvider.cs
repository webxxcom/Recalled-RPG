using System;
using UnityEngine.UI;

namespace Recalled.UI
{
    internal interface IScreenFocusProvider
    {
        /// <summary>
        /// Raised when the last answer from <see cref="TryGetFocusTarget"/> is out of date
        /// (slots built, rebuilt or filtered). Carries no value: ask again.
        /// </summary>
        event Action FocusTargetChanged;

        /// <summary>
        /// The selectable that should receive focus now: the remembered one if still valid,
        /// otherwise the default. Returns false when there is nothing to focus yet.
        /// Computed on every call, so callers must not cache the result.
        /// The manager still validates it (active, interactable, inside the top screen).
        /// </summary>
        bool TryGetFocusTarget(out Selectable target);

        /// <summary>
        /// Called by the focus manager after it selects something inside this screen,
        /// so the provider can remember it. Implementations must never change the selection themselves.
        /// </summary>
        void OnFocused(Selectable selected);
    }
}
