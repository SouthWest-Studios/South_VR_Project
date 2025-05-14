//using UnityEngine;
//using UnityEngine.InputSystem;

//public class RagdollMove : MonoBehaviour
//{
//    public float speed = 5f;
//    public float jumpForce = 5f;

//    public Rigidbody hips;

//    private PlayerInputActions inputActions;
//    private Vector2 moveInput;
//    private bool isSprinting;

//    private void Awake()
//    {
//        inputActions = new PlayerInputActions();

//        //inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
//        //inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

//        //inputActions.Player.Sprint.performed += ctx => isSprinting = true;
//        //inputActions.Player.Sprint.canceled += ctx => isSprinting = false;
//    }

//    private void OnEnable()
//    {
//        inputActions.Enable();
//    }

//    private void OnDisable()
//    {
//        inputActions.Disable();
//    }

//    private void Start()
//    {
//        hips = GetComponent<Rigidbody>();
//    }

//    private void FixedUpdate()
//    {
//        if (moveInput.sqrMagnitude > 0.01f)
//        {
//            float finalSpeed = isSprinting ? speed * 1.5f : speed;


//            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;


//            hips.AddForce(moveDirection * finalSpeed, ForceMode.Force);


//            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
//            hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRotation, 0.1f));
//        }
//    }

//}

using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveRagdollWalker : MonoBehaviour
{
    [Header("References")]
    public Rigidbody hips;
    public ConfigurableJoint spineJoint;
    public ConfigurableJoint leftLegJoint;
    public ConfigurableJoint rightLegJoint;
    public Camera playerCamera;
    public Transform leftFoot;
    public Transform rightFoot; // 需要给脚部添加碰撞体

    [Header("Movement Settings")]
    public float moveForce = 500f;
    public float maxSpeed = 4f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 10f;
    public float groundCheckDistance = 0.2f;

    [Header("Leg Settings")]
    public float stepHeight = 0.3f;
    public float stepSpeed = 4f;
    public float legSwingAngle = 45f;
    public float footGroundForce = 100f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting;
    private bool isLeftLegLeading;
    private float gaitCycle;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void FixedUpdate()
    {
        HandleMovement();
        HandleGaitCycle();
        ApplyFootForces();
    }

    private void HandleMovement()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        // 基于摄像机的移动方向
        Vector3 camForward = playerCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 moveDir = (camForward * moveInput.y + playerCamera.transform.right * moveInput.x).normalized;

        // 施力控制
        float speed = isSprinting ? moveForce * sprintMultiplier : moveForce;
        hips.AddForce(moveDir * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // 速度限制
        Vector3 horizontalVel = new Vector3(hips.velocity.x, 0, hips.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            hips.velocity = new Vector3(horizontalVel.x, hips.velocity.y, horizontalVel.z);
        }

        // 身体转向
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
    }

    private void HandleGaitCycle()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        // 步态周期计算
        gaitCycle += Time.fixedDeltaTime * stepSpeed * (isSprinting ? 1.5f : 1f);
        gaitCycle %= Mathf.PI * 2;

        // 交替迈腿逻辑
        float leftLegPhase = Mathf.Sin(gaitCycle);
        float rightLegPhase = Mathf.Sin(gaitCycle + Mathf.PI);

        // 腿部摆动（绕X轴）
        Vector3 leftSwing = new Vector3(leftLegPhase * legSwingAngle, 0, 0);
        Vector3 rightSwing = new Vector3(rightLegPhase * legSwingAngle, 0, 0);

        // 添加抬腿高度
        leftSwing.y = Mathf.Abs(leftLegPhase) * stepHeight;
        rightSwing.y = Mathf.Abs(rightLegPhase) * stepHeight;

        leftLegJoint.targetRotation = Quaternion.Euler(leftSwing);
        rightLegJoint.targetRotation = Quaternion.Euler(rightSwing);
    }

    private void ApplyFootForces()
    {
        // 检测脚部是否接触地面
        bool leftGrounded = Physics.Raycast(leftFoot.position, Vector3.down, groundCheckDistance);
        bool rightGrounded = Physics.Raycast(rightFoot.position, Vector3.down, groundCheckDistance);

        // 当脚部接触地面时施加反作用力
        if (leftGrounded)
        {
            Vector3 forceDir = (hips.transform.forward + Vector3.up * 0.3f).normalized;
            hips.AddForce(forceDir * footGroundForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        if (rightGrounded)
        {
            Vector3 forceDir = (hips.transform.forward + Vector3.up * 0.3f).normalized;
            hips.AddForce(forceDir * footGroundForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
