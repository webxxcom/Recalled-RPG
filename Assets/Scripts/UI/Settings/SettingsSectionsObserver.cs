using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsSectionsObserver : MonoBehaviour
{
    [SerializeField] List<SettingsSection> _sections;

    private void Awake()
        => _sections = GetComponentsInChildren<SettingsSection>().ToList();
    private void OnEnable()
        => _sections.ForEach(ss => ss.OnStateChange += VerifySingleActiveSection);
    private void OnDisable()
        => _sections.ForEach(ss => ss.OnStateChange -= VerifySingleActiveSection);

    UIScreen _activeSection;
    void VerifySingleActiveSection(UIScreen ss, bool isActive)
    {
        if (!isActive)
        {
            _activeSection = null;
            return;
        }

        if (_activeSection)
            _activeSection.IsActive = false;

        _activeSection = ss;
    }
}
