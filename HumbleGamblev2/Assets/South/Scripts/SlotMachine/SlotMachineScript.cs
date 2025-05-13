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

            StartGame();
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

    public void StartGame()
    {
        int i = 1;
        foreach (GameObject obj in slotCylinders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddTorque(torqueForce, ForceMode.Impulse);
                float drag = Random.Range(0.05f, 0.07f);
                rb.angularDrag = drag / i;
            }
            i++;
        }
        gameRunning = true;
    }

    public void EndGame()
    {
        Dictionary<int, int> resultCounts = new Dictionary<int, int>();
        bool hasWon = false;

        foreach (GameObject obj in slotCylinders)
        {
            int result = obj.GetComponent<SlotCylinderScript>().currentResult;

            if (resultCounts.ContainsKey(result))
            {
                resultCounts[result]++;
            }
            else
            {
                resultCounts[result] = 1;
            }
        }

        foreach (var count in resultCounts.Values)
        {
            if (count >= 3)
            {
                Debug.Log("¡Victoria!");
                hasWon = true;
                break;
            }
        }

        if (!hasWon)
        {
            Debug.Log("Derrota");
        }

        foreach (GameObject obj in slotCylinders)
        {
            obj.GetComponent<SlotCylinderScript>().currentResult = 0;
        }

        gameRunning = false;
    }

}
