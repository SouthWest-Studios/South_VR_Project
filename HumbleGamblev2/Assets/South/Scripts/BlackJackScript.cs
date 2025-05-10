using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static BlackJackCardScript;

public class BlackJackScript : MonoBehaviour
{
    bool gameStarted = true;
    public int currentUserPoints = 0;
    public TextMeshProUGUI userPointsText;
    public int currentClientPoints = 0;
    public TextMeshProUGUI clientPointsText;
    public BlackJackCardScript currentCardInHand;
    private List<BlackJackCardScript> clientCards;
    private List<BlackJackCardScript> userCards;
    public bool clientTurn = true;
    public int betAmount = 0;
    public GameObject cardPrefab;
    public Transform spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        clientCards = new List<BlackJackCardScript>();
        userCards = new List<BlackJackCardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {

            //startGame(0);
            Pull();
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {

            //startGame(0);
            print("iiiiiii");
            AddCardToClient();
        }
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {

            //startGame(0);
            print("iiiiiii");
            AddCardToUser();
        }
    }

    void startGame(int bet)
    {
        betAmount = bet;
    }

    void Stand()
    {
         clientTurn = false;
    }

    void Pull()
    {
        GameObject newCardGO = Instantiate(cardPrefab, spawnPosition.position, Quaternion.identity);
        BlackJackCardScript cardScript = newCardGO.GetComponent<BlackJackCardScript>();
        cardScript.value = RandomValue(1, 11);
        cardScript.type = (CardType)RandomValue(0, 3);
        currentCardInHand = cardScript;
    }

    void ThrowCard()
    {
        currentCardInHand = null;
    }

    public void AddCardToClient()
    {
        if(clientTurn)
        {
            currentClientPoints = currentClientPoints + currentCardInHand.value;
            clientPointsText.text = currentClientPoints.ToString();
            clientCards.Add(currentCardInHand);
            if(currentClientPoints > 21)
            {
                EndGame(true);
            }
            else if(currentClientPoints > 16)
            {
                Stand();
            }
        }
        
    }

    public void RemoveCardToClient()
    {
        if (clientTurn)
        {
            currentClientPoints = currentClientPoints - currentCardInHand.value;
            clientPointsText.text = currentClientPoints.ToString();
            clientCards.Remove(currentCardInHand);

        }

    }

    public void AddCardToUser()
    {
        if (!clientTurn)
        {
            currentUserPoints = currentUserPoints + currentCardInHand.value;
            userPointsText.text = currentUserPoints.ToString();
            userCards.Add(currentCardInHand);
            if (currentUserPoints > 21)
            {
                EndGame(false);
            }
            else if (currentUserPoints > 16)
            {
                if (currentUserPoints > currentClientPoints)
                {
                    EndGame(true);
                }
                else if (currentUserPoints < currentClientPoints)
                {
                    EndGame(false);
                }
            }
        }
    }

    void EndGame(bool dealerWinner)
    {
        if (dealerWinner)
        {
            print("Ha ganado la casa");
        }
        else
        {
            print("Ha ganado el cliente");
        }
        currentClientPoints = 0;
        currentUserPoints = 0;
        userCards.Clear();
        clientCards.Clear();
        clientTurn = true;
    }

    int RandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }


    private void OnTriggerStay(Collider other)
    {
        

    }
}
