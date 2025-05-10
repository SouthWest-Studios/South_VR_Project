using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsZoneSrcipt : MonoBehaviour
{
    // Start is called before the first frame update
    public BlackJackScript blackJackScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.name == "ClientZone")
        {
            if(other.CompareTag("Card"))
            {
                blackJackScript.AddCardToClient();
            }
        }
        else
        {
            if (other.CompareTag("Card"))
            {
                blackJackScript.AddCardToUser();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.name == "ClientZone")
        {
            if (other.CompareTag("Card"))
            {
                blackJackScript.AddCardToClient();
            }
        }
        else
        {
            if (other.CompareTag("Card"))
            {
                blackJackScript.AddCardToUser();
            }
        }
    }
}
