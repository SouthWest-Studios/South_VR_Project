using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [Header("Settings")]
    [Description("En segundos")]
    public float dayTime = 240;
    public int dayTotal = 4;

    public int startingMinutesOffset = 17;

    [Header("Others")]
    public int daysCounter = 0;

    private float timeCounter = 0;

    public static DayManager instance;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= dayTime)
        {
            //Fin del dia
            daysCounter++;
            if (daysCounter >= dayTotal)
            {
                //Pantalla victoria
            }
            else
            {
                //Recargar

            }
        }
    }

    public float GetNormalizedDayTime()
    {
        return timeCounter / dayTime;
    }
    public int GetDayTime()
    {
        return Mathf.CeilToInt(timeCounter);
    }
    public string GetDayTimeString()
    {
        int totalSeconds = GetDayTime();
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        return string.Format("{0:00}:{1:00}", minutes + startingMinutesOffset, seconds);
    }

    public string GetDayString()
    {
        return daysCounter + "/" + dayTotal;
    }
}
