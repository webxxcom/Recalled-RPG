using Recalled.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Recalled.UI
{
    public class FirstSelectableSetter : MonoBehaviour
    {
        [SerializeField] Selectable _selectable;
        [SerializeField] GameObjectRuntimeVariable _currentSelectable;

        public void SetSelectable()
        {
        }
    }
}
