using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotMachineScript : MonoBehaviour
{
    public bool gameRunning = false;

    // Referencias individuales a los hijos con SlotCylinderScript
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;

    private List<GameObject> slotCylinders = new List<GameObject>();
    public Vector3 torqueForce;

    public GameObject cash;

    public AudioSource leverPush;

    private int betAmount = 0;
    void Start()
    {
        SlotCylinderScript[] allSlotScripts = GetComponentsInChildren<SlotCylinderScript>();

        foreach (SlotCylinderScript script in allSlotScripts)
        {
            slotCylinders.Add(script.gameObject);
        }

        if (slotCylinders.Count >= 2)
        {
            slot1 = slotCylinders[0];
            slot2 = slotCylinders[1];
            slot3 = slotCylinders[2];
        }
        else
        {
            Debug.LogError("No hay suficientes SlotCylinderScript encontrados en los descendientes.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {

            StartGame(1);
        }

        bool allResultsSet = true;

        foreach (GameObject obj in slotCylinders)
        {
            int result = obj.GetComponent<SlotCylinderScript>().currentResult;
            if (gameRunning && result != 0)
            {
                print(obj.name + ": " + result);
            }
            else
            {
                allResultsSet = false;
                break;
            }
        }

        if (allResultsSet && gameRunning)
        {
            EndGame();
        }
    }

    public void StartGame(int bet)
    {
        leverPush.Play();
        int i = 1;

        foreach (GameObject obj in slotCylinders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                obj.GetComponent<SlotCylinderScript>().spiningSlotAudio.loop = true;
                obj.GetComponent<SlotCylinderScript>().spiningSlotAudio.Play();
                rb.AddTorque(-torqueForce, ForceMode.Impulse);
                float drag = Random.Range(0.05f, 0.07f);
                rb.angularDrag = drag / i;
            }
            i++;

        }
        gameRunning = true;

        betAmount = bet;
    }

    public void EndGame()
    {
        // currentResult == 2 -- Cereza
        // currentResult == 1 -- Uva
        // currentResult == 3 -- 7
        // currentResult == 4 -- Naranja

        Dictionary<int, int> resultCounts = new Dictionary<int, int>();
        bool hasWon = false;

        List<int> results = new List<int>();

        foreach (GameObject obj in slotCylinders)
        {
            int result = obj.GetComponent<SlotCylinderScript>().currentResult;
            results.Add(result);

            if (resultCounts.ContainsKey(result))
            {
                resultCounts[result]++;
            }
            else
            {
                resultCounts[result] = 1;
            }
        }

        // Comprobar si hay tres sietes
        if (resultCounts.ContainsKey(3) && resultCounts[3] == 3)
        {
            Debug.Log("¡Jackpot! Tres sietes!");
            hasWon = true;
            DayManager.instance.money += betAmount * 5;
            //Instantiate(cash, this.transform.position + new Vector3(5, 0, 5), Quaternion.identity);
        }
        else
        {
            foreach (var pair in resultCounts)
            {
                if ((pair.Key == 1 || pair.Key == 2 || pair.Key == 4) && pair.Value == 3)
                {

                    Debug.Log(" ¡Premio por 3 frutas iguales!");
                    //Instantiate(cash, this.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                    hasWon = true;
                    DayManager.instance.money += betAmount * 3;
                    break;
                }
            }
        }
        // Comprobar si hay tres iguales (frutas o cualquier otro)
        //else
        //{
        //    foreach (var pair in resultCounts)
        //    {
        //        if (pair.Value == 3)
        //        {
        //            Debug.Log("¡Victoria por 3 iguales!");
        //            hasWon = true;
        //            break;
        //        }
        //    }
        //}

        // Comprobar si hay tres frutas diferentes (uva, cereza y naranja)
        if (!hasWon)
        {
            HashSet<int> fruitResults = new HashSet<int>();

            foreach (int result in results)
            {
                if (result == 1 || result == 2 || result == 4) // frutas
                {
                    fruitResults.Add(result);
                }
            }

            if (fruitResults.Count == 3)
            {
                Debug.Log("¡Victoria por 3 frutas diferentes!");
                DayManager.instance.money += betAmount * 2;
                //Instantiate(cash, this.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
                hasWon = true;
            }
        }

        if (!hasWon)
        {
            Debug.Log("Derrota");
            
        }

        // Reset
        foreach (GameObject obj in slotCylinders)
        {
            obj.GetComponent<SlotCylinderScript>().currentResult = 0;
            obj.GetComponent<SlotCylinderScript>().normalSpinDone = false;

            
        }

        gameRunning = false;
    }



}
