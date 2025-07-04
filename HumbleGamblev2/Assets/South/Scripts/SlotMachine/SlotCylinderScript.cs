using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCylinderScript : MonoBehaviour
{
    public AudioSource spiningSlotAudio;
    public enum SlotSymbol
    {
        Symbol0,
        Symbol1,
        Symbol2,
        Symbol3
    }
    public int currentResult = 0;
    Rigidbody rb;
    public SlotMachineScript slotMachineScript;

    public SlotSymbol currentSymbol;

    bool grabing = false;
    bool grabed = false;

    public bool normalSpinDone = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (rb.useGravity == false)
        {
            grabing = true;
            grabed = true;
            normalSpinDone = false;
        }
    }

    public void SetCurrentResult(int result)
    {
        if (rb.angularVelocity.magnitude < 5f && slotMachineScript.gameRunning && rb.useGravity == true && normalSpinDone == false)
        {
            spiningSlotAudio.loop = false;
            spiningSlotAudio.Stop();
            if (rb.angularVelocity.magnitude > 0.5 || grabing)
            {
                currentResult = result;
            }
            
            rb.angularVelocity = Vector3.zero;
            Vector3 eulerRotation = rb.rotation.eulerAngles;


            if (currentResult == 2)
            {
                if (grabed == true)
                {
                    transform.localRotation = Quaternion.Euler(0f, 180, 180f);

                }
                else
                {
                    transform.localRotation = Quaternion.Euler(0f, 180f, 180f);
                }

            }
            else if (currentResult == 1)
            {
                if (grabed == true)
                {
                    transform.localRotation = Quaternion.Euler(180f, 180, 180f);

                }
                else
                {
                    transform.localRotation = Quaternion.Euler(180f, 180f, 180f);
                }
            }
            else if (currentResult == 3)
            {
                if (grabed == true)
                {
                    transform.localRotation = Quaternion.Euler(90f, 180, 180f);

                }
                else
                {
                    transform.localRotation = Quaternion.Euler(89f, 180f, 180f);
                }
            }
            else if (currentResult == 4)
            {
                if (grabed == true)
                {
                    transform.localRotation = Quaternion.Euler(270f, 180, 180f);

                }
                else
                {
                    transform.localRotation = Quaternion.Euler(270f, 180f, 180f);
                }
            }

            normalSpinDone = true;
            grabing = false;
        }

    }

    

    
}
