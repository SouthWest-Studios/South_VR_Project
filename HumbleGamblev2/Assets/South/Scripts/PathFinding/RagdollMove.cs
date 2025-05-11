using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;

    public Rigidbody hips;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Sprint.performed += ctx => isSprinting = true;
        inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (moveInput.sqrMagnitude > 0.01f)
        {
            float finalSpeed = isSprinting ? speed * 1.5f : speed;

            
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            
            hips.AddForce(moveDirection * finalSpeed, ForceMode.Force);

            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRotation, 0.1f));
        }
    }

}
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class ActiveRagdollController : MonoBehaviour
//{
//    [Header("References")]
//    public Rigidbody hips;
//    public ConfigurableJoint spineJoint;
//    public ConfigurableJoint leftLegJoint;
//    public ConfigurableJoint rightLegJoint;

//    [Header("Movement Settings")]
//    public float moveForce = 300f;
//    public float sprintMultiplier = 1.5f;
//    public float legSwingAngle = 25f;
//    public float walkSwingSpeed = 3f;
//    public float sprintSwingSpeed = 6f;

//    private PlayerInputActions inputActions;
//    private Vector2 moveInput;
//    private bool isSprinting;
//    private float currentSwingTime;

//    private void Awake()
//    {
//        inputActions = new PlayerInputActions();

//        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;

//        inputActions.Player.Sprint.performed += _ => isSprinting = true;
//        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
//    }

//    private void OnEnable() => inputActions.Enable();
//    private void OnDisable() => inputActions.Disable();

//    private void FixedUpdate()
//    {
//        HandleMovement();
//        HandleLegSwing();
//        HandleSpineBalance();
//    }

//    private void HandleMovement()
//    {
//        if (moveInput.sqrMagnitude < 0.01f) return;

//        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
//        float speed = isSprinting ? moveForce * sprintMultiplier : moveForce;

//        hips.AddForce(moveDir * speed * Time.fixedDeltaTime, ForceMode.Force);

//        // Optional: make hips face movement direction
//        if (moveDir != Vector3.zero)
//        {
//            Quaternion targetRot = Quaternion.LookRotation(moveDir);
//            hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRot, 0.1f));
//        }
//    }

//    private void HandleLegSwing()
//    {
//        if (moveInput.sqrMagnitude < 0.01f) return;

//        float swingSpeed = isSprinting ? sprintSwingSpeed : walkSwingSpeed;
//        currentSwingTime += Time.fixedDeltaTime * swingSpeed;

//        float swing = Mathf.Sin(currentSwingTime) * legSwingAngle;

//        leftLegJoint.targetRotation = Quaternion.Euler(swing, 0f, 0f);
//        rightLegJoint.targetRotation = Quaternion.Euler(-swing, 0f, 0f);
//    }

//    private void HandleSpineBalance()
//    {
//        // Keep spine leaning slightly forward for walking effect
//        Quaternion forwardTilt = Quaternion.Euler(15f, 0f, 0f);
//        spineJoint.targetRotation = Quaternion.Inverse(forwardTilt);
//    }
//}
