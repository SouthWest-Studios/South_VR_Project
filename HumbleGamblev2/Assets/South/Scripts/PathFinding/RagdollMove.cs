using UnityEngine;
using UnityEngine.AI;

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
    public Transform target;
    public Transform destination2;
    public bool usePathfinding = true;
    public float pathRecalcInterval = 0.5f;
    public float waypointThreshold = 0.5f;
    public float destinationThreshold = 1f;

    private NavMeshPath navPath;
    private int currentCornerIndex = 1;
    private float pathRecalcTimer = 0f;
    private bool hasReachedFirstTarget;
    private Transform currentTarget;
    private float gaitCycle;
    private Vector3 moveDirection;

    void Awake()
    {
        navPath = new NavMeshPath();
        pathRecalcTimer = pathRecalcInterval;
        currentTarget = target;
        hasReachedFirstTarget = false;
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
                    if (!hasReachedFirstTarget)
                    {
                        hasReachedFirstTarget = true;
                        currentTarget = destination2;
                        pathRecalcTimer = 0;
                        currentCornerIndex = 1;
                    }
                    else
                    {
                        usePathfinding = false;
                        Destroy(gameObject);
                        return;
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

    public void SetCurrentTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        pathRecalcTimer = 0;
        currentCornerIndex = 1;
    }
}
