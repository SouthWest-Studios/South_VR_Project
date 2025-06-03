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

    bool firstTime = false;

    void Start()
    {
        //slot1.SetActive(false);
        //slot2.SetActive(false);
        //slot3.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentMoneyText.text = DayManager.instance.money.ToString() + "$";
    }

    public void buyObjects(int num)
    {

        switch (num)
        {
            case 0:
                if(DayManager.instance.money >= 200 && !slot1.activeInHierarchy)
                {
                    slot1.SetActive(true);
                    DayManager.instance.money -= 200;
                }
                break;
            case 1:
                if (DayManager.instance.money >= 300 && !slot2.activeInHierarchy)
                {
                    slot2.SetActive(true);
                    DayManager.instance.money -= 300;
                }
                break;
            case 2:
                if (DayManager.instance.money >= 400 && !slot3.activeInHierarchy)
                {
                    slot3.SetActive(true);
                    DayManager.instance.money -= 400;
                }
                break;
            case 3:
                if (DayManager.instance.money >= 1000)
                {
                    slot3.SetActive(true);
                    DayManager.instance.money -= 1000;
                }
                break;
            default:
                Debug.LogWarning("Número de slot inválido: " + num);
                break;
        }
        

    }
}
