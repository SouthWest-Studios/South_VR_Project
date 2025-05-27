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

    bool grabing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        slotMachineScript = FindAnyObjectByType<SlotMachineScript>();
    }
    void Update()
    {
        if (rb.useGravity == false)
        {
            grabing = true;
        }
    }

    public void SetCurrentResult(int result)
    {
        if (rb.angularVelocity.magnitude < 5f && slotMachineScript.gameRunning && rb.useGravity == true)
        {
            rb.angularVelocity = Vector3.zero;
            currentResult = result;
            print(result);
            Vector3 eulerRotation = rb.rotation.eulerAngles;

            if (grabing == true)
            {
                print("aaaaaaaaaaaaaaaa");
                transform.localRotation = Quaternion.Euler(0f, 90f, 180f);
            }
            else
            {


                if (currentResult == 2)
                {
                    eulerRotation.x = 0f;
                    transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                }
                else if (currentResult == 1)
                {
                    eulerRotation.x = 180;
                    transform.localRotation = Quaternion.Euler(180f, 0f, 180f);
                }
                else if (currentResult == 3)
                {
                    eulerRotation.x = 90f;
                    transform.localRotation = Quaternion.Euler(90f, 0f, 180f);
                }
                else if (currentResult == 4)
                {
                    eulerRotation.x = 270f;
                    transform.localRotation = Quaternion.Euler(270f, 0f, 180f);
                }
            }

        }

    }

    

    
}
