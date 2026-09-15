using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PagesController : MonoBehaviour
{
    [SerializeField] Dictionary<BookUITab, ToggleableObject> _tabToScreenDictionary;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!_tabToScreenDictionary.Any())
        {
            var tabs = GetComponentsInChildren<BookUITab>(true).ToList();
            var screens = GetComponentsInChildren<BookView>(true);

            foreach (var tab in tabs)
                _tabToScreenDictionary[tab] = screens.FirstOrDefault(s => s.name.Equals(tab.name));
        }
    }
#endif
}
