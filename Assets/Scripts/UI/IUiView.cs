using System;
using System.Collections.Generic;
using UnityEngine;

namespace Recalled.UI
{
    interface IUiView
    {
        event Action Changed;
        IReadOnlyList<GameObject> Elements { get; }
    }
}
