using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DelayedGrab : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attachThreshold = 0.05f;

    private XRGrabInteractable grabInteractable;
    private IXRSelectInteractor cachedInteractor;
    private bool isBeingSummoned = false;
    private Transform targetHand;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isBeingSummoned) return;

        cachedInteractor = args.interactorObject;
        targetHand = cachedInteractor.transform;

        // Prevent actual grab for now
        grabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);

        grabInteractable.enabled = false;

        // Start movement coroutine
        StartCoroutine(MoveToHand());
    }

    private IEnumerator MoveToHand()
    {
        isBeingSummoned = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        while (Vector3.Distance(transform.position, targetHand.position) > attachThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetHand.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to hand position (optional)
        transform.position = targetHand.position;

        // Attach point creation
        GameObject tempAttach = new GameObject("TempAttach");
        tempAttach.transform.SetPositionAndRotation(transform.position, transform.rotation);
        tempAttach.transform.SetParent(targetHand);

        grabInteractable.attachTransform = tempAttach.transform;

        // Re-enable grabbing
        grabInteractable.enabled = true;

        // Force the grab
        grabInteractable.interactionManager.SelectEnter(cachedInteractor, grabInteractable);

        isBeingSummoned = false;
    }
}