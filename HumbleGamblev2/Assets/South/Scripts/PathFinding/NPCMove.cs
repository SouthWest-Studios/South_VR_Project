using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class NPCMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerInputActions inputActions;

    [System.Serializable]
    public class Target
    {
        public Transform point;
        public bool isActive = false;
    }

    public List<Target> targets = new List<Target>();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.NPC.GoToTarget0.performed += OnGoToTarget0;
        inputActions.NPC.GoToTarget1.performed += OnGoToTarget1;
    }

    void OnDisable()
    {
        inputActions.NPC.GoToTarget0.performed -= OnGoToTarget0;
        inputActions.NPC.GoToTarget1.performed -= OnGoToTarget1;
        inputActions.Disable();
    }

    void Update()
    {
        foreach (var target in targets)
        {
            if (target.isActive && target.point != null)
            {
                agent.SetDestination(target.point.position);
                target.isActive = false;
                break;
            }
        }
    }

    private void OnGoToTarget0(InputAction.CallbackContext context)
    {
        ActivateTarget(0);
    }

    private void OnGoToTarget1(InputAction.CallbackContext context)
    {
        ActivateTarget(1);
    }

    void ActivateTarget(int index)
    {
        if (index >= 0 && index < targets.Count && targets[index].point != null)
        {
            targets[index].isActive = true;
        }
    }
}
