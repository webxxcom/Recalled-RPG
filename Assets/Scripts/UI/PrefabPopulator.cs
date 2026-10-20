using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPopulator : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parent;

    readonly List<GameObject> _created = new();
    public IReadOnlyList<GameObject> Created => _created;

    public IReadOnlyList<GameObject> Populate(int count)
    {
        DestroyAll();

        for (int i = 0; i < count; i++)
            _created.Add(Instantiate(_prefab, _parent));
        return _created;
    }

    public GameObject Add()
    {
        _created.Add(_prefab);
        return _created[^1];
    }

    public void DestroyAll()
    {
        if (_created == null)
            return;

        foreach (var item in Created)
            Destroy(item);
        _created.Clear();
    }
}
