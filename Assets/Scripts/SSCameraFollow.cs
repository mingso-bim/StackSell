using UnityEngine;

public class SSCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 7f, -7f);

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up - offset);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
    }
}
