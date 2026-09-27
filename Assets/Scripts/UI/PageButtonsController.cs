using UnityEngine;
using UnityEngine.UI;

public class PageButtonsController : MonoBehaviour
{
    [SerializeField] PageController _pageController;
    [SerializeField] Button _nextPageButton;
    [SerializeField] Button _prevPageButton;

    private void OnEnable()
    {
        _pageController.PageChanged += OnPageChanged;
    }
    private void OnDisable()
    {
        _pageController.PageChanged -= OnPageChanged;
    }

    public void OnPageChanged(PageController pageController)
    {
        _nextPageButton.gameObject.SetActive(pageController.HasNextPage);
        _prevPageButton.gameObject.SetActive(pageController.HasPrevPage);
    }
}
