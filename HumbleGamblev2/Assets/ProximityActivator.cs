using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ProximityActivator : MonoBehaviour
{
    [Header("Grab Settings")]
    public float activationDistance = 0.5f;
    [SerializeField] private InputActionProperty gripActionLeft, gripActionRight;
    [SerializeField] private Transform leftController, rightController;
    [SerializeField] private XRBaseInteractor leftInteractor, rightInteractor;

    [Header("Card")]
    public GameObject card;

    private XRGrabInteractable grabInteractable;
    private GameObject spawnedVisual;
    private bool hasLeftPressed = false;
    private bool hasRightPressed = false;

    private void Update()
    {
        // Detect if either hand is close enough to the object
        HandleGripAction(leftInteractor, ref hasLeftPressed, gripActionLeft, leftController);
        HandleGripAction(rightInteractor, ref hasRightPressed, gripActionRight, rightController);
    }

    private void HandleGripAction(XRBaseInteractor interactor, ref bool hasPressed, InputActionProperty gripAction, Transform controller)
    {
        // Calculate the distance between the controller and the object
        float distance = Vector3.Distance(transform.position, controller.position);
        if (distance <= activationDistance)
        {
            if (gripAction.action.ReadValue<float>() > 0.8f)
            {
                if (!hasPressed)
                {
                    hasPressed = true;
                    SpawnVisual();
                    AutoGrab(interactor);
                }
            }
            else
            {
                hasPressed = false;
            }
        }
    }

    private void SpawnVisual()
    {
        // Instantiate the object above the original one
        spawnedVisual = Instantiate(card, transform.position + Vector3.up * 0.1f, Quaternion.identity);
    }

    private void AutoGrab(XRBaseInteractor interactor)
    {
        
        //spawnedVisual.GetComponent<XRGrabInteractable>().selectEntered.Invoke();
        //// Wait a little to ensure the object has spawned
        //yield return new WaitForSeconds(0.05f);

        //grabInteractable = spawnedVisual.GetComponent<XRGrabInteractable>();

        //if (grabInteractable == null || interactor == null)
        //    yield break;

        //grabInteractable.enabled = false;

        //// Force cleanup in case there is any lingering interaction
        //if (interactor.hasSelection)
        //{
        //    interactor.interactionManager.CancelInteractorSelection((IXRSelectInteractor)interactor);
        //}

        //// Clean previous selections
        //grabInteractable.interactionManager.CancelInteractableSelection((IXRSelectInteractable)grabInteractable);

        //yield return null;

        //grabInteractable.enabled = true;

        //// Now perform the grab interaction
        //grabInteractable.interactionManager.SelectEnter(
        //    (IXRSelectInteractor)interactor,
        //    (IXRSelectInteractable)grabInteractable
        //);
    }

}
