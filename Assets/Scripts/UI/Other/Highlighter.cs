using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Highlighter : MonoBehaviour
{
    Image _image;

    private void Awake()
        => _image = GetComponent<Image>();

    public void Show(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<InventorySlot>(out var inventorySlot) && inventorySlot.Item != null)
        {
            _image.enabled = true;
            transform.position = gameObject.transform.position;
        }
    }

    public void Hide()
    {
        _image.enabled = false;
    }
}
