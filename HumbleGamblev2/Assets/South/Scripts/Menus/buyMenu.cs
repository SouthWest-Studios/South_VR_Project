using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class buyMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;

    public TextMeshProUGUI currentMoneyText;

    void Start()
    {
        currentMoneyText.text = DayManager.instance.money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyObjects(int num)
    {

        switch (num)
        {
            case 0:
                slot1.SetActive(true);
                break;
            case 1:
                slot2.SetActive(true);
                break;
            case 2:
                slot3.SetActive(true);
                break;
            case 3: print("juego terminado");
                break;
            default:
                Debug.LogWarning("Número de slot inválido: " + num);
                break;
        }
        

    }
}
