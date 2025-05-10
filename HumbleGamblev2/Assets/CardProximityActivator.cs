using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class CardProximityActivator : MonoBehaviour
{
    [Header("Grab Settings")]
    public float activationDistance = 0.5f; // Distancia para activar la proximidad
    [SerializeField] private InputActionProperty gripActionLeft, gripActionRight; // InputAction para el grip de ambos controladores
    [SerializeField] private Transform leftController, rightController; // Las transformaciones de los controladores
    [SerializeField] private XRBaseInteractor leftInteractor, rightInteractor; // Los interactor de los controladores

    [Header("Card")]
    public GameObject cardPrefab; // El prefab de la carta a aparecer en la mano
    private GameObject spawnedCard; // La carta instanciada en la mano del jugador
    private bool hasActivated = false;

    private void Update()
    {
        // Revisar si el grip se mantiene presionado en el controlador izquierdo
        CheckGripAndProximity(leftInteractor, leftController, ref hasActivated);

        // Revisar si el grip se mantiene presionado en el controlador derecho
        CheckGripAndProximity(rightInteractor, rightController, ref hasActivated);
    }

    private void CheckGripAndProximity(XRBaseInteractor interactor, Transform controller, ref bool hasActivated)
    {
        // Calcular la distancia entre el controlador y la baraja
        float distance = Vector3.Distance(transform.position, controller.position);

        // Verificar si el grip está presionado y si el controlador está cerca
        if (distance <= activationDistance && gripActionLeft.action.ReadValue<float>() > 0.8f)
        {
            if (!hasActivated)
            {
                hasActivated = true;
                SpawnCardInHand(interactor);
            }
        }
        else
        {
            hasActivated = false;
        }
    }

    private void SpawnCardInHand(XRBaseInteractor interactor)
    {
        // Instanciar la carta en la mano del jugador
        spawnedCard = Instantiate(cardPrefab, interactor.transform.position + Vector3.forward * 0.1f, Quaternion.identity);

        // Mover la carta hacia la mano con un movimiento suave
        StartCoroutine(SmoothMoveToHand(interactor));
    }

    private IEnumerator SmoothMoveToHand(XRBaseInteractor interactor)
    {
        // Obtener el XRGrabInteractable de la carta
        XRGrabInteractable grabInteractable = spawnedCard.GetComponent<XRGrabInteractable>();
        grabInteractable.enabled = false; // Deshabilitar la interacción temporalmente

        // Crear un punto de attach temporal en la mano
        Transform attachPoint = new GameObject("TempAttachPoint").transform;
        attachPoint.position = interactor.transform.position;

        // Establecer el attachPoint de la carta a la mano del jugador
        grabInteractable.attachTransform = attachPoint;

        // Mover la carta suavemente hacia la mano del jugador
        while (Vector3.Distance(spawnedCard.transform.position, interactor.transform.position) > 0.01f)
        {
            spawnedCard.transform.position = Vector3.MoveTowards(spawnedCard.transform.position, interactor.transform.position, 10f * Time.deltaTime);
            yield return null;
        }

        // Una vez que la carta ha llegado a la mano, activar el grab interactable
        grabInteractable.enabled = true;

        // Hacer que la carta se agarre automáticamente
        grabInteractable.interactionManager.SelectEnter((IXRSelectInteractor)interactor, (IXRSelectInteractable)grabInteractable);
    }
}
