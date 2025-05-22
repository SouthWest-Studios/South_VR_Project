using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NPCMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentTargetIndex = 0;

    // Hacerlo público para poder accederlo desde otro script
    public bool isInteractingWithTarget = false;
    public bool hasEndedInteractingWithTarget = false;

    GameInteraction gameInteractionScript;

    [System.Serializable]
    public class Target
    {
        public Transform point;
    }

    public List<Target> targets = new List<Target>();
    public float reachThreshold = 0.5f; // distancia mínima para considerar que llegó

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        gameInteractionScript = GetComponent<GameInteraction>();
    }

    void Start()
    {
        if (targets.Count > 0 && targets[0].point != null)
        {
            agent.SetDestination(targets[0].point.position);
        }
    }

    void Update()
    {
        if (targets.Count == 0) return;

        // Si el NPC ha llegado al destino y no está en interacción
        if (!agent.pathPending && agent.remainingDistance <= reachThreshold && !isInteractingWithTarget)
        {
            isInteractingWithTarget = true;  // Marca como interactuando con el waypoint actual
            hasEndedInteractingWithTarget = false;
        }

        // Cuando ha interactuado (llegado) y se le permite pasar al siguiente waypoint
        if (isInteractingWithTarget && hasEndedInteractingWithTarget)
        {
            // Se puede esperar un poco o hacer alguna acción, según la lógica
            // Luego, pasa al siguiente waypoint
            currentTargetIndex = (currentTargetIndex + 1) % targets.Count;
            agent.SetDestination(targets[currentTargetIndex].point.position);
        }
    }

    // Método para cambiar el estado de la interacción desde otro script
    public void SetInteractingWithTarget(bool state)
    {
        isInteractingWithTarget = state;
    }
}
