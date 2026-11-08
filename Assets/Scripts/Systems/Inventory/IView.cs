using System;
using UnityEngine;

namespace Recalled.Systems.Inventory
{
    public interface IView
    {
        event Action Changed;
    }
}
