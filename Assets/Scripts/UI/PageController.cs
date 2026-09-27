using System;
using TMPro;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public bool HasNextPage => true;
    public bool HasPrevPage => _textMeshPro.pageToDisplay > 1;

    public Action<PageController> PageChanged;

    private void OnEnable()
    {
        _isDirty = true;
    }

    public void NextPage()
    {
        if (_textMeshPro.overflowMode != TextOverflowModes.Page || !HasNextPage)
            return;

        _textMeshPro.pageToDisplay++;
        _isDirty = true;
    }
    public void PrevPage()
    {
        if (_textMeshPro.overflowMode != TextOverflowModes.Page || !HasPrevPage)
            return;

        _textMeshPro.pageToDisplay--;
        _isDirty = true;
    }

    bool _isDirty;
    private void LateUpdate()
    {
        if (_isDirty)
        {
            _isDirty = false;
            PageChanged?.Invoke(this);
        }
    }
}
