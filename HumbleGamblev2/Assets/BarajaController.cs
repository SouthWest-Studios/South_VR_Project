using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BarajaController : MonoBehaviour
{
    [Header("Grab Settings")]
    public float activationDistance = 0.5f;
    [SerializeField] private InputActionProperty gripActionLeft, gripActionRight;
    [SerializeField] private Transform leftController, rightController;
    [SerializeField] private XRBaseInteractor leftInteractor, rightInteractor;

    [Header("Card")]
    public GameObject card;
    private GameObject spawnedCard;

    private bool hasLeftPressed = false;
    private bool hasRightPressed = false;

    private bool prevLeftPressed = false;
    private bool prevRightPressed = false;

    BlackJackScript blackJackScript;

    public AudioSource drawCardSound;

    // Start is called before the first frame update
    void Start()
    {
        blackJackScript = FindAnyObjectByType<BlackJackScript>();
    }

    // Update is called once per frame
    void Update()
    {
        

  
        HandleGripAction(leftInteractor, ref hasLeftPressed, ref prevLeftPressed, gripActionLeft, leftController);
        HandleGripAction(rightInteractor, ref hasRightPressed, ref prevRightPressed, gripActionRight, rightController);

        prevLeftPressed = (gripActionLeft.action.ReadValue<float>() > 0.8f);
        prevRightPressed = (gripActionRight.action.ReadValue<float>() > 0.8f);

    }

    private void HandleGripAction(XRBaseInteractor interactor, ref bool hasPressed,ref bool prevPressed, InputActionProperty gripAction, Transform controller)
    {
        // Calculate the distance between the controller and the object

        float distance = Vector3.Distance(transform.position, controller.position);
        if (distance <= activationDistance || hasPressed)
        {
            if (gripAction.action.ReadValue<float>() > 0.8f)
            {
                Debug.Log("haspressed: " + hasPressed + " __ prevpressed: " + prevPressed);
                if (!hasPressed && !prevPressed)
                {
                    hasPressed = true;
                    spawnedCard = blackJackScript.Pull();
                    //AutoGrab(interactor);
                    GrabCard(interactor);
                }
            }
            else
            {
                if (hasPressed)
                {
                    ReleaseCard(interactor);
                    hasPressed = false;
                }
                
            }
        }
    }

    void GrabCard(XRBaseInteractor interactor)
    {
        drawCardSound.Play();
        SelectEnterEventArgs events = new SelectEnterEventArgs();
        events.interactorObject = interactor;


        spawnedCard.GetComponent<SmoothGrabMovement>().OnGrab(events);
        spawnedCard.GetComponent<DelayedGrab>().OnSelectEntered(events);
    }

    void ReleaseCard(XRBaseInteractor interactor)
    {
        SelectExitEventArgs events = new SelectExitEventArgs();
        events.interactorObject = interactor;
        spawnedCard.GetComponent<SmoothGrabMovement>().OnRelease(events);
    }
}
