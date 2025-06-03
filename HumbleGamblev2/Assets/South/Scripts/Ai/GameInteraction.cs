using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteraction : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rb;
    int moneyToBet;
    int minMoneyToBet = 15;
    int maxMoneyToBet = 25;

    NPCMove nPCMoveScript;

    string currentGame = "";

    public ActiveRagdollWalker activeRagdollWalker;

    public GameObject blackJackPointer;

    void Start()
    {
        blackJackPointer.SetActive(false);
        rb = this.GetComponent<Rigidbody>();

        nPCMoveScript = GetComponent<NPCMove>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlackJackTable"))
        {
            BlackJackScript script = other.gameObject.GetComponent<BlackJackScript>();
            if (script.gameStarted == false && currentGame != "BlackJackTable")
            {
                moneyToBet = UnityEngine.Random.Range(minMoneyToBet, maxMoneyToBet + 1);
                script.startGame(this.GetComponent<GameInteraction>(), moneyToBet);
                blackJackPointer.SetActive(true);
                currentGame = "BlackJackTable";
                activeRagdollWalker.enabled = false;
            }
        }
        else if (other.CompareTag("Slot"))
        {
            SlotMachineScript script = other.gameObject.GetComponent<SlotMachineScript>();
            if (script.gameRunning == false && currentGame != "Slot")
            {
                moneyToBet = UnityEngine.Random.Range(minMoneyToBet, maxMoneyToBet + 1);
                script.StartGame(this.GetComponent<GameInteraction>(), moneyToBet);
                currentGame = "Slot";
                activeRagdollWalker.enabled = false;
            }
        }
    }

    public void EndGameInteraction()
    {
        activeRagdollWalker.enabled = true;
        blackJackPointer.SetActive(false);
    }
}
