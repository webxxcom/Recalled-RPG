using System;
using UnityEngine;

namespace Recalled.Gameplay
{
    [Serializable]
    public class RuntimeBoolCheck : ICondition
    {
        [SerializeField] BoolRuntimeVariable _variable;

        public bool Check() => _variable.Value;
    }
}
