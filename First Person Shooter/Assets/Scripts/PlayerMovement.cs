using System.Collections;
using UnityEngine;

public enum SimpleStateMachine
{
    Idle,
    Moving,
    Jumping,
    Shooting
}

public class PlayerMovement : MonoBehaviour
{
    [Header("State Machine")]
    public SimpleStateMachine currentState;

    [Header("Player Settings")]
    public Transform playerMainCamera;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleInputs();
        RotateCamera();

        switch (currentState)
        {
            case SimpleStateMachine.Idle:
                moveDirection = GetMoveDirection(movementInput);

                if (moveDirection.x != 0f && moveDirection.z != 0f)
                    currentState = SimpleStateMachine.Moving;

                SetGravity();

                if (jumpInput && isGrounded)
                {
                    UseJump();
                    currentState = SimpleStateMachine.Jumping;
                }

                characterController.Move(moveDirection * Time.deltaTime);
                break;
            case SimpleStateMachine.Moving:
                moveDirection = GetMoveDirection(movementInput);

                if (moveDirection.x == 0f && moveDirection.z == 0f)
                    currentState = SimpleStateMachine.Idle;

                SetGravity();
                
                if (jumpInput && isGrounded)
                {
                    UseJump();
                    currentState = SimpleStateMachine.Jumping;
                }

                characterController.Move(moveDirection * Time.deltaTime);
                break;
            case SimpleStateMachine.Jumping:
                moveDirection = GetMoveDirection(movementInput);
                SetGravity();
                characterController.Move(moveDirection * Time.deltaTime);

                if (isGrounded)
                {
                    currentState = SimpleStateMachine.Moving;
                }
                break;
        }
    }

    #region Inputs

    [Header("Debug Inputs")]
    public Vector3 movementInput;
    public Vector3 mouseInput;
    public bool isGrounded;
    public bool jumpInput;

    private void HandleInputs()
    {
        // Use PlayerInputController's Singleton
        movementInput = PlayerInputController.instance.Current.MoveInputRaw;

        mouseInput = PlayerInputController.instance.Current.MouseInput;

        jumpInput = PlayerInputController.instance.Current.JumpInput;

        isGrounded = GetGroundedStatus();
    }

    #endregion

    #region Camera Rotation and Look

    [Header("Camera Rotation and Look")]
    public float sensitivity = 100f;
    public float xRotation = 0f;

    private void RotateCamera()
    {
        float mouseX = mouseInput.x * sensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerMainCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    #endregion

    #region Movement and Rotation

    [Header("Player Movement and Rotation")]
    public float movementSpeed = 10f;

    [Header("Movement Debug")]
    public Vector3 moveDirection;
    public Vector3 camForward;
    public Vector3 camRight;

    private void GetCamDirection()
    {
        camForward = playerMainCamera.forward;
        camRight = playerMainCamera.right;

        camForward.y = 0;
        camRight.y = 0;
    }

    private Vector3 GetMoveDirection(Vector3 input)
    {
        GetCamDirection();

        return (input.x * camRight + input.z * camForward) * movementSpeed;
    }

    #endregion

    #region Abilities

    [Header("Abilities")]
    public float jumpForce = 15f;

    private void UseJump()
    {
        fallVelocity = jumpForce;
        moveDirection.y = fallVelocity;
    }

    #endregion

    #region Gravity and Ground Detection

    [Header("Gravity Settings")]
    public float gravity = 40f;
    public float slideVelocity = 7f;
    public float slopeForceDown = 10f;

    [Header("Ground Time Remember")]
    public float groundedRememberTime = 0.1f;

    [Header("Debug Gravity and Ground Detection")]
    public Vector3 hitNormal;
    public float fallVelocity;
    public bool isOnSlope;
    public float groundedRemember;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    private void SetGravity()
    {
        if (characterController.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            moveDirection.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            moveDirection.y = fallVelocity;
        }

        SlideDown();
    }

    private void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) > characterController.slopeLimit;

        if (isOnSlope)
        {
            moveDirection.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            moveDirection.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;
            moveDirection.y -= slopeForceDown;
        }
    }

    private bool GetGroundedStatus()
    {
        groundedRemember -= groundedRemember > 0 ? Time.deltaTime : 0;

        if (characterController.isGrounded)
        {
            groundedRemember = groundedRememberTime;
        }

        return groundedRemember > 0f;
    }

    #endregion
}
