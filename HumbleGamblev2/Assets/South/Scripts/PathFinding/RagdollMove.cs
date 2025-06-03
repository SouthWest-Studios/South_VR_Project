using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;  // 引入 NavMesh
using System.Collections.Generic;

public class ActiveRagdollWalker : MonoBehaviour
{
    [Header("References")]
    public Rigidbody hips;
    public ConfigurableJoint spineJoint;
    public ConfigurableJoint leftLegJoint;
    public ConfigurableJoint rightLegJoint;
    public Rigidbody leftFootRb;
    public Rigidbody rightFootRb;
    public Transform leftFoot;
    public Transform rightFoot;

    [Header("Movement Settings")]
    public float moveForce = 500f;
    public float maxSpeed = 4f;
    public float rotationSpeed = 10f;
    public float groundCheckDistance = 0.2f;

    [Header("Leg Settings")]
    public float stepHeight = 6f;
    public float stepSpeed = 4f;
    public float legSwingAngle = 30f;
    public float footGroundForce = 150f;

    [Header("Pathfinding Settings")]
    [Tooltip("Lista de destinos a alcanzar en orden")]
    public List<Transform> targets = new List<Transform>();
    [Tooltip("是否启用自动寻路")]
    public bool usePathfinding = true;     // 开关：true时自动沿路径移动，false则依赖手动输入
    [Tooltip("路径重算时间间隔（秒）")]
    public float pathRecalcInterval = 0.5f;
    public float waypointThreshold = 0.5f;
    public float destinationThreshold = 1f;

    private NavMeshPath navPath;
    private int currentCornerIndex = 1;
    private float pathRecalcTimer = 0f;
    private int currentTargetIndex = 0;
    private Transform currentTarget => (currentTargetIndex < targets.Count) ? targets[currentTargetIndex] : null;


    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isSprinting;
    private float gaitCycle;
    private Vector3 moveDirection;


    void Awake()
    {
        navPath = new NavMeshPath();
        pathRecalcTimer = pathRecalcInterval;

        if (targets.Count == 0)
        {
            //usePathfinding = false;
            Debug.LogWarning("No targets assigned!");
        }

    }

    void FixedUpdate()
    {
        if (usePathfinding && currentTarget != null)
        {
            UpdatePath();

            if (navPath != null && navPath.corners.Length > 0)
            {
                Vector3 targetPos = navPath.corners[currentCornerIndex];
                Vector3 diff = targetPos - hips.position;
                diff.y = 0f;
                float distance = diff.magnitude;

                if (distance < waypointThreshold && currentCornerIndex < navPath.corners.Length - 1)
                {
                    currentCornerIndex++;
                    targetPos = navPath.corners[currentCornerIndex];
                    diff = targetPos - hips.position;
                    diff.y = 0f;
                    distance = diff.magnitude;
                }

                Vector3 finalDestination = navPath.corners[navPath.corners.Length - 1];
                float finalDistance = Vector3.Distance(
                    new Vector3(hips.position.x, 0, hips.position.z),
                    new Vector3(finalDestination.x, 0, finalDestination.z)
                );

                if (finalDistance < destinationThreshold)
                {
                    if (finalDistance < destinationThreshold)
                    {
                        currentTargetIndex++;

                        if (currentTargetIndex >= targets.Count)
                        {
                            usePathfinding = false;
                            Destroy(gameObject);
                            return;
                        }

                        pathRecalcTimer = 0f; // recalcula de inmediato
                        currentCornerIndex = 1;
                    }

                }
                else
                {
                    moveDirection = diff.normalized;
                }
            }
        }

        HandleMovement();
        HandleGaitCycle();
        ApplyFootForces();
    }

    private void UpdatePath()
    {
        pathRecalcTimer -= Time.fixedDeltaTime;
        if (pathRecalcTimer <= 0f && currentTarget != null)
        {
            NavMeshPath newPath = new NavMeshPath();
            if (NavMesh.CalculatePath(hips.position, currentTarget.position, NavMesh.AllAreas, newPath))
            {
                navPath = newPath;
                currentCornerIndex = 1;
            }
            pathRecalcTimer = pathRecalcInterval;
        }
    }

    private void HandleMovement()
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        hips.AddForce(moveDirection * moveForce * Time.fixedDeltaTime, ForceMode.VelocityChange);

        Vector3 horizontalVel = new Vector3(hips.velocity.x, 0, hips.velocity.z);
        if (horizontalVel.magnitude > maxSpeed)
        {
            horizontalVel = horizontalVel.normalized * maxSpeed;
            hips.velocity = new Vector3(horizontalVel.x, hips.velocity.y, horizontalVel.z);
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            hips.MoveRotation(Quaternion.Slerp(hips.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    private void HandleGaitCycle()
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        gaitCycle += Time.fixedDeltaTime * stepSpeed;
        gaitCycle %= Mathf.PI * 2;

        float leftLegPhase = Mathf.Sin(gaitCycle);
        float rightLegPhase = Mathf.Sin(gaitCycle + Mathf.PI);

        Vector3 leftSwing = new Vector3(leftLegPhase * legSwingAngle, 0, 0);
        Vector3 rightSwing = new Vector3(rightLegPhase * legSwingAngle, 0, 0);

        leftLegJoint.targetRotation = Quaternion.Euler(leftSwing);
        rightLegJoint.targetRotation = Quaternion.Euler(rightSwing);

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

    public void SetCurrentTargetByIndex(int index)
    {
        if (targets != null && index >= 0 && index < targets.Count)
        {
            currentTargetIndex = index;
        }
        else
        {
            Debug.LogWarning("Índice fuera de rango o lista de targets vacía.");
        }
    }


}
