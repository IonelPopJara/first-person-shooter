using System.Collections;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController instance = null;

    public PlayerInput Current;

    // Agregar cool downs here
    public float jumpPressedRememberTime = 0.25f;
    public float jumpPressedRemember;

    private void Awake()
    {
        //Initiate Singleton
        if (instance == null)
            instance = this;
        else if (instance != this) 
            Destroy(gameObject);
    }
    private void Start()
    {
        Current = new PlayerInput();
    }

    private void Update()
    {
        Vector3 moveInputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        jumpPressedRemember -= jumpPressedRemember > 0 ? Time.deltaTime : 0;

        // Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedRemember = jumpPressedRememberTime;
        }

        bool interactInput = Input.GetKeyDown(KeyCode.F);

        bool shootInput = Input.GetKeyDown(KeyCode.Mouse0);
        bool shootHoldInput = Input.GetKey(KeyCode.Mouse0);

        bool aimInput = Input.GetKeyDown(KeyCode.Mouse1);

        bool dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        Current = new PlayerInput()
        {
            MoveInputRaw = moveInputRaw,
            MoveInput = moveInput,
            MouseInput = mouseInput,
            JumpInput = jumpPressedRemember > 0f,
            ShootInput = shootInput,
            ShootHoldInput = shootHoldInput,
            AimInput = aimInput,
            InteractInput = interactInput,
            DashInput = dashInput,
        };
    }
    public struct PlayerInput
    {
        public Vector3 MoveInputRaw;
        public Vector3 MoveInput;
        public Vector2 MouseInput;
        public bool JumpInput;
        public bool ShootInput;
        public bool ShootHoldInput;
        public bool AimInput;
        public bool InteractInput;
        public bool DashInput;
    }
}