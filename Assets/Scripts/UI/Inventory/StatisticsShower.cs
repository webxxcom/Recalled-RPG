using TMPro;
using UnityEngine;

public class StatisticsShower : MonoBehaviour
{
    [SerializeField] PlayerCombatData _playerData;
    [SerializeField] TextMeshProUGUI _damageTextMesh;
    [SerializeField] TextMeshProUGUI _protectionTextMesh;
    [SerializeField] TextMeshProUGUI _weightTextMesh;
    [SerializeField] TextMeshProUGUI _knockbackTextMesh;
    [SerializeField] TextMeshProUGUI _reloadTextMesh;
    [SerializeField] InventoryManager _inventoryManager;

    void Awake()
        => Refresh();
    private void OnEnable()
        => _inventoryManager.OnEquippedItems += Refresh;
    private void OnDisable()
        => _inventoryManager.OnEquippedItems -= Refresh;

    void Refresh()
    {
        _damageTextMesh.text = _playerData.DealtDamage.ToString("F2");
        _protectionTextMesh.text = _playerData.Protection.ToString("F2");
        _weightTextMesh.text = _playerData.Weight.ToString("F2");
        _knockbackTextMesh.text = _playerData.KnockbackPower.ToString("F2");
        _reloadTextMesh.text = _playerData.ReloadTime.ToString("F2");
    }
}
