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
    [Min(1f)]
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

        // handleRange가 0이면 0으로 나눠 InputVector가 NaN이 되므로 최소 1로 막는다.
        float range = Mathf.Max(1f, handleRange);

        Vector2 direction = Vector2.ClampMagnitude(localPoint / range, 1f);

        handle.anchoredPosition = direction * range;
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

    // 드래그 도중 비활성화되면 OnPointerUp이 오지 않아 InputVector와 조이스틱 표시가
    // 그대로 남는다. 플레이어가 계속 이동하지 않도록 표시 상태까지 초기화한다.
    private void OnDisable()
    {
        InputVector = Vector2.zero;

        if (background != null)
            background.gameObject.SetActive(false);

        if (handle != null)
            handle.gameObject.SetActive(false);
    }
}
