using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class AutoScrollToSelection : MonoBehaviour
{
    private ScrollRect _scrollRect;
    private RectTransform _contentRect;
    private RectTransform _viewportRect;
    private GameObject _lastSelectedObject;

    void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _contentRect = _scrollRect.content;
        _viewportRect = _scrollRect.viewport;
    }

    void Update()
    {
        // 1. What is currently highlighted?
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        // 2. Do nothing if no change, or if we navigated outside the shop list
        if (!selected || selected == _lastSelectedObject) return;
        if (!selected.transform.IsChildOf(_contentRect)) return;

        _lastSelectedObject = selected;

        // 3. Move the scroll view smoothly!
        ScrollToTarget((RectTransform)selected.transform);
    }

    private void ScrollToTarget(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        float contentHeight = _contentRect.rect.height;
        float viewportHeight = _viewportRect.rect.height;

        // If all items already fit perfectly on the screen, don't try to scroll!
        if (contentHeight <= viewportHeight) return;

        // Find how far down the list this item is (Anchored Y is usually negative, so we make it absolute)
        float targetY = Mathf.Abs(target.anchoredPosition.y);

        // Convert that pixel distance into a percentage (0.0 to 1.0)
        // 1f is the top of the scroll view, 0f is the bottom.
        float normalizedY = 1f - (targetY / (contentHeight - viewportHeight));

        // Clamp the value to ensure we don't accidentally scroll past the limits
        _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(normalizedY);
    }
}