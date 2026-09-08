using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI.Settings.Persistence
{
    public abstract class SettingsConfigField<T> : MonoBehaviour
    {
        [SerializeField] protected SettingsConfig _settingsConfig;
        [SerializeField, Dropdown(nameof(Fields))] protected string _fieldName;
        string[] Fields => SettingsDataService.Fields;

        protected abstract T Value { get; set; }
        protected abstract UnityEvent<T> Event { get; }

        void OnEnable()
        {
            Event.AddListener(OnUiValueChanged);
            _settingsConfig.OnSettingsLoaded += UpdateFieldView;
        }
        void OnDisable()
        {
            Event.RemoveListener(OnUiValueChanged);
            _settingsConfig.OnSettingsLoaded -= UpdateFieldView;
        }

        void OnUiValueChanged(T val)
            => _settingsConfig.SettingsData.SetField(_fieldName, val);
        void UpdateFieldView()
            => Value = (T)_settingsConfig.SettingsData.GetFieldValue(_fieldName);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!SettingsDataService.FindField(_fieldName).FieldType.IsEquivalentTo(Value.GetType()))
                throw new UnityException($"Type mismatch of settings field '{gameObject.name}' and set '{_fieldName}'");
        }
#endif
    }
}
