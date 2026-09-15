using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PagesController : MonoBehaviour
{
    [SerializeField] ScreenGroup _tabs;
    [SerializeField] ScreenGroup _pages;
    [SerializeField] Dictionary<BookUITab, BookView> _tabToScreenDictionary = new();

    private void OnEnable()
    {
        _tabs.ScreenChanged += OnTabChange;
    }
    private void OnDisable()
    {
        _tabs.ScreenChanged -= OnTabChange;
    }

    void OnTabChange(ToggleableObject tab)
    {
        if (_tabToScreenDictionary.TryGetValue((BookUITab)tab, out var bookView))
        {
            if (!_pages.RequestScreen(bookView))
                throw new System.Exception($"Invalid operation of toggling a {nameof(BookView)} on {nameof(PagesController)}");
        }
    }

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
