using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Recalled.UI
{
    /// <summary>
    /// 
    /// Component used to group a set of gameobject which have the <strong>ToggleableScreen</strong> component.
    /// The discovering happens only at the immediate children to avoid grouping the gameobjects from down the hierarchy.
    /// 
    /// The main purpose of this component is to provide an activation of screens within a group using an event
    ///     <strong>ToggleableScreen.Toggled</strong>. The component itself only manages whether
    ///     the screen should be turned on or not which wholly depends whether the screen
    ///     is active or multiple screens are allowed
    /// 
    /// </summary>
    sealed class ToggleableGroup : MonoBehaviour
    {
        [SerializeField] bool _allowEmpty;
        [SerializeField] bool _allowMultiple;

        List<Toggleable> _toggleables;
        public IReadOnlyList<Toggleable> Elements
        {
            get
            {
                if (_toggleables == null)
                {
                    _toggleables = new();
                    foreach (Transform child in transform)
                    {
                        if (child.TryGetComponent(out Toggleable screen))
                            _toggleables.Add(screen);
                    }
                }
                return _toggleables;
            }
        }

        public event Action<IToggleable> ActiveChanged;

        private void OnEnable()
        {
            foreach (var toggleable in Elements)
                toggleable.ToggleRequested += Toggle;
        }
        private void OnDisable()
        {
            foreach (var toggleable in Elements)
                toggleable.ToggleRequested -= Toggle;
        }

        void Toggle(Toggleable toggleable)
        {
            if (toggleable.IsActive) RemoveActiveToggleable(toggleable);
            else AddActiveToggleable(toggleable);
        }


        bool AddActiveToggleable(IToggleable toggleable)
        {
            // Don't want to set same again or more than one if disallowed
            if (toggleable == null || toggleable.IsActive)
                return false;

            IToggleable firstActive = _toggleables.FirstOrDefault(t => t.IsActive);
            if (!_allowMultiple && firstActive != null)
                firstActive.SetIsActive(false);

            toggleable.SetIsActive(true);
            ActiveChanged?.Invoke(toggleable);
            return true;
        }

        bool RemoveActiveToggleable(IToggleable toggleable)
        {
            if (toggleable == null || !toggleable.IsActive || (!_allowEmpty && !_toggleables.Any(t => t.IsActive)))
                return false;

            toggleable.SetIsActive(false);
            ActiveChanged?.Invoke(_toggleables.LastOrDefault(t => t.IsActive));
            return true;
        }

        public void ChooseNext(int offset)
        {
            if (offset < 0) offset += _toggleables.Count;
                
            offset %= _toggleables.Count;
            int i = 0;
            for (; i < _toggleables.Count; i++)
                if (_toggleables[i].IsActive)
                    break;

            var nextTab = _toggleables[(i + offset) % _toggleables.Count];
            AddActiveToggleable(nextTab);
        }
    }
}
