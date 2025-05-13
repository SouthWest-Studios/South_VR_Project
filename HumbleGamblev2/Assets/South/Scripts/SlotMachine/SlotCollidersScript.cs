using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCollidersScript : MonoBehaviour
{
    public int slot;
    public SlotCylinderScript slotCylinderScript;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Result"))
        {
            slotCylinderScript.SetCurrentResult(slot);
        }
        
    }
}
