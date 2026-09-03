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
        _image.enabled = true;
        transform.position = gameObject.transform.position;
    }

    public void Hide()
    {
        _image.enabled = false;
    }
}
