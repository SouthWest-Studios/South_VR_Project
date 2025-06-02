using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Smartwatch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI dayText;
    // Start is called before the first frame update

    private DayManager dm;

    void Start()
    {
        dm = DayManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = dm.GetDayTimeString();
        dayText.text = dm.GetDayString();
    }
}
