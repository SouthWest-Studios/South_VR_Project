using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TIPCurrentMoney : MonoBehaviour
{
    public TextMeshPro textTip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textTip.text = DayManager.instance.money.ToString() + "$";
        if (DayManager.instance.money > 0)
        {
            textTip.color = DayManager.instance.greenMoneyColor;
        }
        else
        {
            textTip.color = DayManager.instance.redMoneyColor;
        }
    }
}
