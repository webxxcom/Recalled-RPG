using System;
using TMPro;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [SerializeField] TMP_Text _tmpText;

    public bool HasNextPage => _tmpText.pageToDisplay < _tmpText.textInfo.pageCount;
    public bool HasPrevPage => _tmpText.pageToDisplay > 1;

    public Action<PageController> PageChanged;

    public void NextPage()
    {
        if (_tmpText.overflowMode != TextOverflowModes.Page || !HasNextPage)
            return;

        _tmpText.pageToDisplay++;
        _isDirty = true;
    }
    public void PrevPage()
    {
        if (_tmpText.overflowMode != TextOverflowModes.Page || !HasPrevPage)
            return;

        _tmpText.pageToDisplay--;
        _isDirty = true;
    }

    string _prevText;
    private void Update()
    {
        if (_tmpText.text != _prevText)
        {
            _prevText = _tmpText.text;
            _tmpText.pageToDisplay = 1;
            PageChanged?.Invoke(this);
        }
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
