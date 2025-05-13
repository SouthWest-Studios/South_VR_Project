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
    public Texture2D[] clubsTextures;
    public Texture2D[] diamondsTextures;
    public Texture2D[] heartsTextures;

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

        clubsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        clubsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_A");  // CL_A = Índice 0
        clubsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_2");  // CL_2 = Índice 1
        clubsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_3");  // CL_3 = Índice 2
        clubsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_4");  // CL_4 = Índice 3
        clubsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_5");  // CL_5 = Índice 4
        clubsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_6");  // CL_6 = Índice 5
        clubsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_7");  // CL_7 = Índice 6
        clubsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_8");  // CL_8 = Índice 7
        clubsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_9");  // CL_9 = Índice 8
        clubsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_10"); // CL_10 = Índice 9
        clubsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_J"); // CL_J = Índice 10
        clubsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_Q"); // CL_Q = Índice 11
        clubsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_K"); // CL_K = Índice 12

        diamondsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        diamondsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_A");  // DI_A = Índice 0
        diamondsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_2");  // DI_2 = Índice 1
        diamondsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_3");  // DI_3 = Índice 2
        diamondsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_4");  // DI_4 = Índice 3
        diamondsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_5");  // DI_5 = Índice 4
        diamondsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_6");  // DI_6 = Índice 5
        diamondsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_7");  // DI_7 = Índice 6
        diamondsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_8");  // DI_8 = Índice 7
        diamondsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_9");  // DI_9 = Índice 8
        diamondsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_10"); // DI_10 = Índice 9
        diamondsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_J"); // DI_J = Índice 10
        diamondsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_Q"); // DI_Q = Índice 11
        diamondsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_K"); // DI_K = Índice 12

        heartsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        heartsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_A");  // HE_A = Índice 0
        heartsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_2");  // HE_2 = Índice 1
        heartsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_3");  // HE_3 = Índice 2
        heartsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_4");  // HE_4 = Índice 3
        heartsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_5");  // HE_5 = Índice 4
        heartsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_6");  // HE_6 = Índice 5
        heartsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_7");  // HE_7 = Índice 6
        heartsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_8");  // HE_8 = Índice 7
        heartsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_9");  // HE_9 = Índice 8
        heartsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_10"); // HE_10 = Índice 9
        heartsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_J"); // HE_J = Índice 10
        heartsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_Q"); // HE_Q = Índice 11
        heartsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_K"); // HE_K = Índice 12

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

    public GameObject Pull()
    {
        GameObject newCardGO = Instantiate(cardPrefab, spawnPosition.position, Quaternion.identity);
        BlackJackCardScript cardScript = newCardGO.GetComponent<BlackJackCardScript>();
        newCardGO.GetComponent<BlackJackCardScript>().id = RandomValue(1, 13);
        if (newCardGO.GetComponent<BlackJackCardScript>().id < 11 )
        {
            newCardGO.GetComponent<BlackJackCardScript>().value = newCardGO.GetComponent<BlackJackCardScript>().id;
        }
        else
        {
            newCardGO.GetComponent<BlackJackCardScript>().value = 10;
        }
        newCardGO.GetComponent<BlackJackCardScript>().type = (CardType)RandomValue(0, 3);
        //currentCardInHand = cardScript;

        return newCardGO;
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
