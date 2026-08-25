using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SSPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null)
            return;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        Vector3 moveDirection =
            new Vector3(input.x, 0f, input.y).normalized;

        characterController.Move(
            moveDirection * moveSpeed * Time.deltaTime
        );

        if (moveDirection.sqrMagnitude > 0f)
        {
            transform.forward = moveDirection;
        }
    }
}
