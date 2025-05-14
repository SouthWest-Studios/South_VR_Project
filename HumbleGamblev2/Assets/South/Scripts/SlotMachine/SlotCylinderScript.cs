using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCylinderScript : MonoBehaviour
{
    public enum SlotSymbol
    {
        Symbol0,
        Symbol1,
        Symbol2,
        Symbol3
    }
    public int currentResult = 0;
    Rigidbody rb;
    SlotMachineScript slotMachineScript;

    public SlotSymbol currentSymbol;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        slotMachineScript = FindAnyObjectByType<SlotMachineScript>();
    }
    void Update()
    {
        
    }

    public void SetCurrentResult(int result)
    {
        if (rb.angularVelocity.magnitude < 5f && slotMachineScript.gameRunning && rb.useGravity == true)
        {
            rb.angularVelocity = Vector3.zero;
            currentResult = result;
            print(result);
            Vector3 eulerRotation = rb.rotation.eulerAngles;

            if (currentResult == 2)
            {
                eulerRotation.x = 0f;
                transform.localEulerAngles = new Vector3(0f, 0f, 180f);
            }
            else if (currentResult == 1)
            {
                eulerRotation.x = 180;
                transform.localEulerAngles = new Vector3(180f, 0f, 180f);
            }
            else if (currentResult == 3)
            {
                eulerRotation.x = 90f;
                transform.localEulerAngles = new Vector3(90f, 0f, 180f);
            }
            else if (currentResult == 4)
            {
                eulerRotation.x = 270f;
                transform.localEulerAngles = new Vector3(270f, 0f, 180f);
            }

        }

    }

    

    
}
