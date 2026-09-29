using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class SpriteRendererGroup : MonoBehaviour
{
    static readonly int NameID = Shader.PropertyToID("_Color");

    SpriteRenderer[] _spriteRenderers;
    MaterialPropertyBlock _materialPropertyBlock;
    SortingGroup _sortingGroup;

    private void Awake()
    {
        _sortingGroup = GetComponent<SortingGroup>();
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _materialPropertyBlock = new();
    }

    void SetAll()
    {
        foreach (var sr in _spriteRenderers)
            sr.SetPropertyBlock(_materialPropertyBlock);
    }

    Color AlphaColor(float alpha) => new(1f, 1f, 1f, alpha);
    public void SetAlpha(float alpha)
    {
        _materialPropertyBlock.SetColor(NameID, AlphaColor(alpha));
        SetAll();
    }

    public void SetColor(Color col)
    {
        _materialPropertyBlock.SetColor(NameID, col);
        SetAll();
    }

    public void SetSortingOrder(int order)
    {
        _sortingGroup.sortingOrder = order;
    }

    public void SetFlipX(bool flag)
    {
        foreach (var sr in _spriteRenderers)
            sr.flipX = flag;
    }
}
