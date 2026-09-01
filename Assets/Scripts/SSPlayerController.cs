using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class SSPlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private SSFloatingJoystick floatingJoystick;

    public float MoveSpeed => moveSpeed;

    private CharacterController characterController;

    [SerializeField]
    private Animator animator;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    private void HandleMovement()
    {
        Vector2 keyboardInput = ReadKeyboardInput();
        Vector2 joystickInput = floatingJoystick != null ? floatingJoystick.InputVector : Vector2.zero;

        Vector2 input = Vector2.ClampMagnitude(keyboardInput + joystickInput, 1f);

        animator.SetFloat("Speed", input.magnitude);

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

    private Vector2 ReadKeyboardInput()
    {
        if (Keyboard.current == null)
            return Vector2.zero;

        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1f;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1f;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1f;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1f;

        return input;
    }
}
