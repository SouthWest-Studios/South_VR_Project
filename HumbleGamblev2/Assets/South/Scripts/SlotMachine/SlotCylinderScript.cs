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
        if (rb.angularVelocity.magnitude < 5f && slotMachineScript.gameRunning)
        {
            rb.angularVelocity = Vector3.zero;
            currentResult = result;
            print(result);
            Vector3 eulerRotation = rb.rotation.eulerAngles;

            if (currentResult == 2)
            {
                eulerRotation.x = 0f;

                rb.MoveRotation(Quaternion.Euler(eulerRotation));
            }
            else if (currentResult == 1)
            {
                eulerRotation.x = 180;

                rb.MoveRotation(Quaternion.Euler(eulerRotation));
            }
            else if (currentResult == 3)
            {
                eulerRotation.x = 90f;

                rb.MoveRotation(Quaternion.Euler(eulerRotation));
            }
            else if (currentResult == 4)
            {
                eulerRotation.x = 270f;

                rb.MoveRotation(Quaternion.Euler(eulerRotation));
            }
        }

    }

    

    
}
