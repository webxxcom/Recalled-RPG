using UnityEngine;

[CreateAssetMenu(menuName = "Behavior/Constant")]
public class ConstantTargetProviderSO : TargetProviderSO
{
    [SerializeField] GameObject _target;

    public GameObject Target => _target;
   
    public override TargetProvider CreateInstance()
        => new ConstantTargetProvider().Init(this);
}
