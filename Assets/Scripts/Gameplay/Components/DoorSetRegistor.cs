using Recalled.Core;
using UnityEngine;

namespace Recalled.Gameplay
{
    [RequireComponent(typeof(Door))]
    public class DoorSetRegistor : AutoSetRegistor<Door>
    {
        protected override Door Data => GetComponent<Door>();
    }
}
