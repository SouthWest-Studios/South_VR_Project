using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;  // 引入 NavMesh

public class ActiveRagdollWalker : MonoBehaviour
{
    [Header("References")]
    public Rigidbody hips;
    public ConfigurableJoint spineJoint;
    public ConfigurableJoint leftLegJoint;
    public ConfigurableJoint rightLegJoint;

    public Rigidbody leftFootRb;
    public Rigidbody rightFootRb;

    public Camera playerCamera;
    public Transform leftFoot;
    public Transform rightFoot;

    [Header("Movement Settings")]
    public float moveForce = 500f;
    public float maxSpeed = 4f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 10f;
    public float groundCheckDistance = 0.2f;

    [Header("Leg Settings")]
    public float stepHeight = 6f;            // 脚步抬起力度
    public float stepSpeed = 4f;             // 步态速度
    public float legSwingAngle = 30f;        // 腿前后摆角度
    public float footGroundForce = 150f;     // 落地反作用力

    [Header("Pathfinding Settings")]
    [Tooltip("通往目标的GameObject的Transform")]
    public Transform target;               // 目标位置
    [Tooltip("是否启用自动寻路")]
    public bool usePathfinding = true;     // 开关：true时自动沿路径移动，false则依赖手动输入
    [Tooltip("路径重算时间间隔（秒）")]
    public float pathRecalcInterval = 0.5f;
    [Tooltip("到达航点的距离阈值")]
    public float waypointThreshold = 0.5f;

    private NavMeshPath navPath;
    private int currentCornerIndex = 1;
    private float pathRecalcTimer = 0f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting;
    private float gaitCycle;

    void Awake()
    {
        // 初始化输入系统
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;

        // 初始化路径
        navPath = new NavMeshPath();
        pathRecalcTimer = pathRecalcInterval;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void FixedUpdate()
    {
        // 如果启用了寻路且目标存在，则通过NavMesh计算路径并生成移动方向
        if (usePathfinding && target != null)
        {
            UpdatePath();

            // 当路径有效时，依次沿航点前进
            if (navPath != null && navPath.corners.Length > 0)
            {
                Vector3 targetPos = navPath.corners[currentCornerIndex];
                Vector3 diff = targetPos - hips.position;
                diff.y = 0f; // 忽略高度差，保持水平方向移动
                float distance = diff.magnitude;

                // 如果接近当前航点，则切换到下一个航点（如果存在）
                if (distance < waypointThreshold && currentCornerIndex < navPath.corners.Length - 1)
                {
                    currentCornerIndex++;
                }

                // 计算从当前刚体位置到该航点的方向
                Vector3 pathDirection = diff.normalized;

                // 将世界方向转换为相对于摄像头的输入（与已有的移动计算逻辑保持一致）
                float xInput = Vector3.Dot(pathDirection, playerCamera.transform.right);
                float yInput = Vector3.Dot(pathDirection, playerCamera.transform.forward);
                moveInput = new Vector2(xInput, yInput);
            }
        }
        // 若未启用自动寻路，则依靠手动输入，moveInput由输入系统更新

        HandleMovement();
        HandleGaitCycle();
        ApplyFootForces();
    }

    /// <summary>
    /// 根据当前刚体位置与目标位置计算导航路径，每隔一定时间重算一次
    /// </summary>
    private void UpdatePath()
    {
        pathRecalcTimer -= Time.fixedDeltaTime;
        if (pathRecalcTimer <= 0f && target != null)
        {
            NavMeshPath newPath = new NavMeshPath();
            // 计算从 hips 的当前位置到 target 的路径
            if (NavMesh.CalculatePath(hips.position, target.position, NavMesh.AllAreas, newPath))
            {
                navPath = newPath;
                currentCornerIndex = 1; // 重置为第一个航点（corners[0]通常为起点）
            }
            pathRecalcTimer = pathRecalcInterval;
        }
    }

    private void HandleMovement()
    {
        // 若输入为空则不移动
        if (moveInput.sqrMagnitude < 0.01f) return;

        // 以摄像头正方向（忽略Y轴）获得基本移动方向
        Vector3 camForward = playerCamera.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + playerCamera.transform.right * moveInput.x).normalized;
        float speed = isSprinting ? moveForce * sprintMultiplier : moveForce;

        hips.AddForce(moveDir * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // 限制水平方向速度
        Vector3 horizontalVel = new Vector3(hips.velocity.x, 0, hips.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            hips.velocity = new Vector3(horizontalVel.x, hips.velocity.y, horizontalVel.z);
        }

        // 使刚体朝向运动方向旋转
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
    }

    private void HandleGaitCycle()
    {
        if (moveInput.sqrMagnitude < 0.01f) return;

        gaitCycle += Time.fixedDeltaTime * stepSpeed * (isSprinting ? 1.5f : 1f);
        gaitCycle %= Mathf.PI * 2;

        // 通过正弦值确定左右腿的动画相位
        float leftLegPhase = Mathf.Sin(gaitCycle);
        float rightLegPhase = Mathf.Sin(gaitCycle + Mathf.PI);

        Vector3 leftSwing = new Vector3(leftLegPhase * legSwingAngle, 0, 0);
        Vector3 rightSwing = new Vector3(rightLegPhase * legSwingAngle, 0, 0);

        leftLegJoint.targetRotation = Quaternion.Euler(leftSwing);
        rightLegJoint.targetRotation = Quaternion.Euler(rightSwing);

        // 抬脚时给予脚部向上和向前推的力
        if (leftLegPhase > 0f && leftFootRb != null)
        {
            Vector3 lift = Vector3.up * stepHeight + hips.transform.forward * 1.5f;
            leftFootRb.AddForce(lift, ForceMode.Acceleration);
        }

        if (rightLegPhase > 0f && rightFootRb != null)
        {
            Vector3 lift = Vector3.up * stepHeight + hips.transform.forward * 1.5f;
            rightFootRb.AddForce(lift, ForceMode.Acceleration);
        }
    }

    private void ApplyFootForces()
    {
        // 检查左右脚是否接触地面
        bool leftGrounded = Physics.Raycast(leftFoot.position, Vector3.down, groundCheckDistance);
        bool rightGrounded = Physics.Raycast(rightFoot.position, Vector3.down, groundCheckDistance);

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
