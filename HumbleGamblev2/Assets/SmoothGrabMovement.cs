using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SmoothGrabMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float stopDistance = 0.01f;

    private XRGrabInteractable grabInteractable;
    private Transform targetHand;
    private bool isMovingToHand = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        targetHand = args.interactorObject.transform;
        isMovingToHand = true;

        // Desactiva la física
        GetComponent<Rigidbody>().isKinematic = true;

        // Crear un punto de attachment temporal en la mano
        Transform interactorTransform = args.interactorObject.transform;
        GameObject attachPoint = new GameObject("TempAttachPoint");
        attachPoint.transform.position = transform.position;
        attachPoint.transform.rotation = transform.rotation;
        attachPoint.transform.SetParent(interactorTransform);

        grabInteractable.attachTransform = attachPoint.transform;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isMovingToHand = false;
        GetComponent<Rigidbody>().isKinematic = false;

        // Limpia el punto de attach temporal
        if (grabInteractable.attachTransform != null)
        {
            Destroy(grabInteractable.attachTransform.gameObject);
            grabInteractable.attachTransform = null;
        }
    }

    void Update()
    {
        if (isMovingToHand && targetHand != null)
        {
            // Movimiento suave hacia la mano
            transform.position = Vector3.MoveTowards(transform.position, targetHand.position, moveSpeed * Time.deltaTime);

            // Cuando llega, detén el movimiento
            if (Vector3.Distance(transform.position, targetHand.position) < stopDistance)
            {
                isMovingToHand = false;
            }
        }
    }
}
