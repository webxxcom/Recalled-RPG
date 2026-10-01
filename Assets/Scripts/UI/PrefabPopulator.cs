using System.Collections.Generic;
using UnityEngine;

public class PrefabPopulator : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parent;

    public IReadOnlyList<GameObject> Created { get; private set; }

    public IReadOnlyList<GameObject> Populate(int count)
    {
        var slots = new GameObject[count];
        for (int i = 0; i < count; i++)
            slots[i] = Instantiate(_prefab, _parent);
        Created = slots;
        return Created;
    }

    public void DestroyAll()
    {
        foreach (var item in Created)
            Destroy(item);
        Created = null;
    }
}
