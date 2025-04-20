using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillTreeZoomAndPan : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Header("References")]
    public RectTransform target;
    public RectTransform viewport;
    public RectTransform background;

    [Header("Zoom Settings")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2.5f;

    [Header("Parallax Settings")]
    public float parallaxFactor = 0.5f;

    private Vector2 lastPointerPosition;
    private bool isZooming = false;
    private bool wasZoomingLastFrame = false;


    void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            var touch0 = Touchscreen.current.touches[0];
            var touch1 = Touchscreen.current.touches[1];

            if (touch0.isInProgress && touch1.isInProgress)
            {
                Vector2 pos0 = touch0.position.ReadValue();
                Vector2 pos1 = touch1.position.ReadValue();

                Vector2 prevPos0 = pos0 - touch0.delta.ReadValue();
                Vector2 prevPos1 = pos1 - touch1.delta.ReadValue();

                float prevDistance = Vector2.Distance(prevPos0, prevPos1);
                float currentDistance = Vector2.Distance(pos0, pos1);

                float delta = currentDistance - prevDistance;
                float scaleFactor = 1 + delta * zoomSpeed * Time.deltaTime;

                ApplyZoom(scaleFactor);
                isZooming = true;
            }
            else
            {
                isZooming = false;
            }
        }
        else
        {
            isZooming = false;
        }

        UpdateParallax();

        if (wasZoomingLastFrame && !isZooming)
        {
            if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
            {
                lastPointerPosition = Touchscreen.current.touches[0].position.ReadValue();
            }
        }
        wasZoomingLastFrame = isZooming;
    }

    private void ApplyZoom(float scaleFactor)
    {
        Vector3 newScale = target.localScale * scaleFactor;
        newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
        newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
        target.localScale = newScale;

        ClampPosition();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isZooming)
        {
            lastPointerPosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isZooming)
        {
            Vector2 delta = eventData.position - lastPointerPosition;
            lastPointerPosition = eventData.position;

            target.anchoredPosition += delta;
            ClampPosition();
        }
    }

    private void ClampPosition()
    {
        Vector2 min, max;
        GetPanLimits(out min, out max);

        Vector2 clamped = target.anchoredPosition;
        clamped.x = Mathf.Clamp(clamped.x, min.x, max.x);
        clamped.y = Mathf.Clamp(clamped.y, min.y, max.y);

        target.anchoredPosition = clamped;
    }

    private void GetPanLimits(out Vector2 min, out Vector2 max)
    {
        Vector2 viewportSize = viewport.rect.size;
        Vector2 contentSize = Vector2.Scale(target.rect.size, target.localScale);

        float limitX = Mathf.Max(0, (contentSize.x - viewportSize.x) / 2f);
        float limitY = Mathf.Max(0, (contentSize.y - viewportSize.y) / 2f);

        min = new Vector2(-limitX, -limitY);
        max = new Vector2(limitX, limitY);
    }

    private void UpdateParallax()
    {
        if (background != null && target != null)
        {
            background.anchoredPosition = target.anchoredPosition * parallaxFactor;
        }
    }
}
