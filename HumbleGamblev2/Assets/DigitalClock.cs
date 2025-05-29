using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
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
    }
}
