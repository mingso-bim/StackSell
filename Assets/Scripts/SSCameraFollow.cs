using UnityEngine;

public class SSCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 7f, -7f);

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning($"{name}: target이 설정되지 않아 카메라를 따라가지 않습니다.");
            return;
        }

        transform.position = target.position + offset;
    }
}
