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

    [SerializeField] private InputActionProperty gripAction;

    // Start is called before the first frame update
    void Start()
    {
        clientCards = new List<BlackJackCardScript>();
        userCards = new List<BlackJackCardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //float gripValue = gripAction.action.ReadValue<float>();
        //if(gripValue > 0)
        //{
        //    Pull();
        //}
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {

            //startGame(0);
            Pull();
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
        //currentCardInHand = cardScript;
    }

    void ThrowCard()
    {
        //currentCardInHand = null;
    }

    public void AddCardToClient(BlackJackCardScript currentCard)
    {
        if(clientTurn)
        {
            currentClientPoints = currentClientPoints + currentCard.value;
            clientPointsText.text = currentClientPoints.ToString();
            clientCards.Add(currentCard);
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

    public void RemoveCardToClient(BlackJackCardScript currentCard)
    {
        if (clientTurn)
        {
            currentClientPoints = currentClientPoints - currentCard.value;
            clientPointsText.text = currentClientPoints.ToString();
            clientCards.Remove(currentCard);

        }

    }

    public void AddCardToUser(BlackJackCardScript currentCard)
    {
        if (!clientTurn)
        {
            currentUserPoints = currentUserPoints + currentCard.value;
            userPointsText.text = currentUserPoints.ToString();
            userCards.Add(currentCard);
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

    public void RemoveCardToUser(BlackJackCardScript currentCard)
    {
        if (!clientTurn)
        {
            currentUserPoints = currentUserPoints - currentCard.value;
            userPointsText.text = currentUserPoints.ToString();
            userCards.Remove(currentCard);
            
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
