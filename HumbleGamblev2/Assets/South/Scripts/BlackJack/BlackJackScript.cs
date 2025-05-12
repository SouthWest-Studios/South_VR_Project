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

    public Texture2D[] spadesTextures;

    // Start is called before the first frame update
    void Start()
    {
        clientCards = new List<BlackJackCardScript>();
        userCards = new List<BlackJackCardScript>();

        spadesTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        spadesTextures[0] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_A");  // SP_A = Índice 0
        spadesTextures[1] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_2");  // SP_2 = Índice 1
        spadesTextures[2] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_3");  // SP_3 = Índice 2
        spadesTextures[3] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_4");  // SP_4 = Índice 3
        spadesTextures[4] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_5");  // SP_5 = Índice 4
        spadesTextures[5] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_6");  // SP_6 = Índice 5
        spadesTextures[6] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_7");  // SP_7 = Índice 6
        spadesTextures[7] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_8");  // SP_8 = Índice 7
        spadesTextures[8] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_9");  // SP_9 = Índice 8
        spadesTextures[9] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_10"); // SP_10 = Índice 9
        spadesTextures[10] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_J"); // SP_J = Índice 10
        spadesTextures[11] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_Q"); // SP_Q = Índice 11
        spadesTextures[12] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_K"); // SP_K = Índice 12

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
        cardScript.id = RandomValue(1, 13);
        if (cardScript.id < 11 )
        {
            cardScript.value = cardScript.id;
        }
        else
        {
            cardScript.value = 10;
        }
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
