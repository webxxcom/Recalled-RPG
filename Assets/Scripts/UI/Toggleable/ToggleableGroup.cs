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
    public class ToggleableGroup : MonoBehaviour
    {
        [SerializeField] bool _allowEmpty;
        [SerializeField] bool _allowMultiple; 

        List<Toggleable> _toggleables;
        protected IReadOnlyList<IToggleable> Active => _toggleables.Where(t => t.IsActive).ToArray();

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
            foreach (var screen in Elements)
                screen.ToggleRequested += Toggle;
        }

        private void OnDisable()
        {
            foreach (var screen in Elements)
                screen.ToggleRequested -= Toggle;
        }

        void Toggle(Toggleable toggleable)
        {
            if (Active.Contains(toggleable)) TryRemoveActiveToggleable(toggleable);
            else AddActiveToggleable(toggleable);
        }

        protected virtual bool AddActiveToggleable(IToggleable toggleable)
        {
            // Don't want to set same again or more than one if disallowed
            if (toggleable == null || toggleable.IsActive)
                return false;

            if (!_allowMultiple && Active.Any())
                Active.First().SetIsActive(false);

            toggleable.SetIsActive(true);
            ActiveChanged?.Invoke(toggleable);
            return true;
        }

        protected virtual bool TryRemoveActiveToggleable(IToggleable toggleable)
        {
            if (toggleable == null || !toggleable.IsActive || (!_allowEmpty && Active.Count <= 1))
                return false;

            toggleable.SetIsActive(false);
            return true;
        }

        public void ChooseNext(int offset)
        {
            if (offset < 0) offset %= _toggleables.Count;

            int i = 0;
            for (; i < _toggleables.Count; i++)
                if (_toggleables[i].IsActive)
                    break;

            var nextTab = _toggleables[(i + offset) % _toggleables.Count];
            AddActiveToggleable(nextTab);
        }

        public bool TryAddActive(IToggleable toggleable)
        {
            if (toggleable == null || !_toggleables.Contains(toggleable))
                return false;

            AddActiveToggleable(toggleable);
            return true;
        }

        public bool TryRemoveActive(Toggleable toggleable)
        {
            if (toggleable == null || !_toggleables.Contains(toggleable))
                return false;

            TryRemoveActiveToggleable(toggleable);
            return true;
        }
    }
}
