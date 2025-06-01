using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static BlackJackCardScript;

public class BlackJackScript : MonoBehaviour
{
    public bool gameStarted = true;
    public int currentUserPoints = 0;
    public TextMeshPro userPointsText;
    public int currentClientPoints = 0;
    public TextMeshPro clientPointsText;
    public BlackJackCardScript currentCardInHand;
    private List<BlackJackCardScript> clientCards;
    private List<BlackJackCardScript> userCards;
    public List<GameObject> cardGameObjects;
    public bool clientTurn = true;
    public int betAmount = 0;
    public GameObject cardPrefab;
    public Transform spawnPosition;

    [SerializeField] private InputActionProperty gripAction;

    public Texture2D[] spadesTextures;
    public Texture2D[] clubsTextures;
    public Texture2D[] diamondsTextures;
    public Texture2D[] heartsTextures;

    public GameObject cash;

    GameInteraction currentClient;


    [Header("Tips")]
    public GameObject tip_PlaceCard_Crupier;
    public GameObject tip_PlaceCard_Client;
    private int currentMoney;

    // Start is called before the first frame update
    void Start()
    {
        clientPointsText.text = "";
        tip_PlaceCard_Client.SetActive(false);
        tip_PlaceCard_Crupier.SetActive(false);

        currentMoney = DayManager.instance.money;
        clientCards = new List<BlackJackCardScript>();
        userCards = new List<BlackJackCardScript>();

        spadesTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        spadesTextures[0] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_A");  // SP_A = �ndice 0
        spadesTextures[1] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_2");  // SP_2 = �ndice 1
        spadesTextures[2] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_3");  // SP_3 = �ndice 2
        spadesTextures[3] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_4");  // SP_4 = �ndice 3
        spadesTextures[4] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_5");  // SP_5 = �ndice 4
        spadesTextures[5] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_6");  // SP_6 = �ndice 5
        spadesTextures[6] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_7");  // SP_7 = �ndice 6
        spadesTextures[7] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_8");  // SP_8 = �ndice 7
        spadesTextures[8] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_9");  // SP_9 = �ndice 8
        spadesTextures[9] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_10"); // SP_10 = �ndice 9
        spadesTextures[10] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_J"); // SP_J = �ndice 10
        spadesTextures[11] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_Q"); // SP_Q = �ndice 11
        spadesTextures[12] = Resources.Load<Texture2D>("Cards/Types/Spades/SP_K"); // SP_K = �ndice 12

        clubsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        clubsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_A");  // CL_A = �ndice 0
        clubsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_2");  // CL_2 = �ndice 1
        clubsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_3");  // CL_3 = �ndice 2
        clubsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_4");  // CL_4 = �ndice 3
        clubsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_5");  // CL_5 = �ndice 4
        clubsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_6");  // CL_6 = �ndice 5
        clubsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_7");  // CL_7 = �ndice 6
        clubsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_8");  // CL_8 = �ndice 7
        clubsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_9");  // CL_9 = �ndice 8
        clubsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_10"); // CL_10 = �ndice 9
        clubsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_J"); // CL_J = �ndice 10
        clubsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_Q"); // CL_Q = �ndice 11
        clubsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Clubs/CL_K"); // CL_K = �ndice 12

        diamondsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        diamondsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_A");  // DI_A = �ndice 0
        diamondsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_2");  // DI_2 = �ndice 1
        diamondsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_3");  // DI_3 = �ndice 2
        diamondsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_4");  // DI_4 = �ndice 3
        diamondsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_5");  // DI_5 = �ndice 4
        diamondsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_6");  // DI_6 = �ndice 5
        diamondsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_7");  // DI_7 = �ndice 6
        diamondsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_8");  // DI_8 = �ndice 7
        diamondsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_9");  // DI_9 = �ndice 8
        diamondsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_10"); // DI_10 = �ndice 9
        diamondsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_J"); // DI_J = �ndice 10
        diamondsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_Q"); // DI_Q = �ndice 11
        diamondsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Diamonds/DI_K"); // DI_K = �ndice 12

        heartsTextures = new Texture2D[13]; // 13 texturas, de 0 a 12
        heartsTextures[0] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_A");  // HE_A = �ndice 0
        heartsTextures[1] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_2");  // HE_2 = �ndice 1
        heartsTextures[2] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_3");  // HE_3 = �ndice 2
        heartsTextures[3] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_4");  // HE_4 = �ndice 3
        heartsTextures[4] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_5");  // HE_5 = �ndice 4
        heartsTextures[5] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_6");  // HE_6 = �ndice 5
        heartsTextures[6] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_7");  // HE_7 = �ndice 6
        heartsTextures[7] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_8");  // HE_8 = �ndice 7
        heartsTextures[8] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_9");  // HE_9 = �ndice 8
        heartsTextures[9] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_10"); // HE_10 = �ndice 9
        heartsTextures[10] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_J"); // HE_J = �ndice 10
        heartsTextures[11] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_Q"); // HE_Q = �ndice 11
        heartsTextures[12] = Resources.Load<Texture2D>("Cards/Types/Hearts/HE_K"); // HE_K = �ndice 12

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

    public void startGame(GameInteraction client, int bet)
    {
        tip_PlaceCard_Client.SetActive(true);
        tip_PlaceCard_Crupier.SetActive(false);
        betAmount = bet;
        gameStarted = true;
        currentClient = client;
    }

    void Stand()
    {
        clientTurn = false;
        tip_PlaceCard_Client.SetActive(false);
        tip_PlaceCard_Crupier.SetActive(true);
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
        cardGameObjects.Add(newCardGO);

        return newCardGO;
        
    }

    void ThrowCard()
    {
        //currentCardInHand = null;
    }

    public void AddCardToClient(BlackJackCardScript currentCard)
    {
        if(gameStarted)
        {
            if (clientTurn)
            {
                currentClientPoints = currentClientPoints + currentCard.value;
                clientPointsText.text = currentClientPoints.ToString();
                clientCards.Add(currentCard);
                if (currentClientPoints > 21)
                {
                    EndGame(true);
                }
                else if (currentClientPoints > 16)
                {
                    Stand();
                }
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
            currentMoney = currentMoney + betAmount;
        }
        else
        {
            print("Ha ganado el cliente");
            currentMoney = currentMoney - betAmount * 2;
        }
        currentClientPoints = 0;
        currentUserPoints = 0;
        userCards.Clear();
        clientCards.Clear();
        betAmount = 0;
        foreach (GameObject card in cardGameObjects)
        {
            GameObject.Destroy(card); // o DestroyImmediate(card); si est�s en el editor
        }

        cardGameObjects.Clear();
        clientTurn = true;
        gameStarted = false;
        currentClient.EndGameInteraction();
        currentClient = null;
    }

    int RandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }


    private void OnTriggerStay(Collider other)
    {
        

    }
}
