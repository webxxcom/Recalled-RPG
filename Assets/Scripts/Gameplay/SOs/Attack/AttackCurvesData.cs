using UnityEngine;

[CreateAssetMenu(menuName = "ApplyAttack / ApplyAttack _curves Value")]
public class AttackCurvesSO : ScriptableObject
{
    [field: SerializeField] public AnimationCurve ColliderSizeX { get; private set; }
    [field: SerializeField] public AnimationCurve ColliderSizeY { get; private set; }
    [field: SerializeField] public AnimationCurve ColliderOffsetX { get; private set; }
    [field: SerializeField] public AnimationCurve ColliderOffsetY { get; private set; }
}
