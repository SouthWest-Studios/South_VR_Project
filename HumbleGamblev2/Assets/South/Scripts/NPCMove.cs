using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NPCMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.NPC.MoveCommand.performed += OnMoveCommand;
        inputActions.NPC.StopCommand.performed += OnStopCommand;
    }

    private void OnDisable()
    {
        inputActions.NPC.MoveCommand.performed -= OnMoveCommand;
        inputActions.NPC.StopCommand.performed -= OnStopCommand;
        inputActions.Disable();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnMoveCommand(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.point);
        }
    }

    private void OnStopCommand(InputAction.CallbackContext context)
    {
        agent.isStopped = true;
    }
}
