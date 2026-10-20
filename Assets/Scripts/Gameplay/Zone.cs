using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class Zone : MonoBehaviour
    {
        public Collider2D Collider2D { get; private set; }

        public event Action<GameObject> Entered;
        public event Action<GameObject> Left;

        private void Awake()
        {
            Collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Entered?.Invoke(collision.gameObject);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Left?.Invoke(collision.gameObject);
        }
    }
}
