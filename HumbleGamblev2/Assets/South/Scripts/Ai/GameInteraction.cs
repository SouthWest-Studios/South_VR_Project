using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rb;
    int moneyToBet;
    int minMoneyToBet = 20;
    int maxMoneyToBet = 100;

    NPCMove nPCMoveScript;

    string currentGame = "";

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();

        nPCMoveScript = GetComponent<NPCMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("BlackJackTable"))
        {
            BlackJackScript script = other.gameObject.GetComponent<BlackJackScript>();
            if(!script.gameStarted && currentGame != "BlackJackTable")
            {
                moneyToBet = UnityEngine.Random.Range(minMoneyToBet, maxMoneyToBet + 1);
                script.startGame(moneyToBet);
                currentGame = other.tag;
            }
        }
    }

    public void EndGameInteraction()
    {
        nPCMoveScript.hasEndedInteractingWithTarget = true;
    }
}
