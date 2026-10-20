using System.Collections.Generic;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Zone : MonoBehaviour
    {
        public Collider2D Collider2D { get; private set; }

        private void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }
    }
}
