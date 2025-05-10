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
        if (rb.angularVelocity.magnitude < 0.02f && slotMachineScript.gameRunning)
        {
            rb.angularVelocity = Vector3.zero;
            currentResult = result;
        }

    }

    

    
}
