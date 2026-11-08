using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    public class DialogueChoicesView : MonoBehaviour, IUiView
    {
        public IReadOnlyList<GameObject> Elements => throw new NotImplementedException();

        public event Action Changed;
    }
}
