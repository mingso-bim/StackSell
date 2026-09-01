using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class SSFloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform background;

    [SerializeField]
    private RectTransform handle;

    [SerializeField]
    private float handleRange = 100f;

    public Vector2 InputVector { get; private set; }

    private RectTransform touchAreaRect;

    private void Awake()
    {
        touchAreaRect = (RectTransform)transform;

        if (background != null)
            background.gameObject.SetActive(false);

        if (handle != null)
            handle.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (background == null || handle == null)
        {
            Debug.LogWarning($"{name}: background 또는 handle이 설정되지 않아 Joystick을 표시할 수 없습니다.");
            return;
        }

        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            touchAreaRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        background.anchoredPosition = localPoint;
        handle.anchoredPosition = Vector2.zero;
        InputVector = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        Vector2 direction = Vector2.ClampMagnitude(localPoint / handleRange, 1f);

        handle.anchoredPosition = direction * handleRange;
        InputVector = direction;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector2.zero;

        if (background != null)
            background.gameObject.SetActive(false);

        if (handle != null)
            handle.gameObject.SetActive(false);
    }
}
