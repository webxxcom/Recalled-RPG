using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    public sealed class PrefabPopulator : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] Transform _parent;

        readonly List<GameObject> _created = new();
        public IReadOnlyList<GameObject> Created => _created;

        public IReadOnlyList<GameObject> Populate(int count)
        {
            RemoveAll();

            for (int i = 0; i < count; i++) Add();
            return _created;
        }

        public GameObject Add()
        {
            _created.Add(Instantiate(_prefab, _parent));

            return _created[^1];
        }

        public void RemoveAt(int i)
        {
            if (i < 0 || i >= _created.Count)
                return;

            _created[i].SetActive(false);
            Destroy(_created[i]);
            _created.RemoveAt(i);
        }

        public void RemoveAfterAt(int startingInd)
        {
            for (int i = startingInd; i < _created.Count; i++)
                RemoveAt(i);
        }

        public void RemoveAll()
        {
            for (int i = 0; i < _created.Count; i++)
                RemoveAt(i);

            _created.Clear();
        }
    }
}
