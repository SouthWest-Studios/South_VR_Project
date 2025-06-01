using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{

    [Header("References")]
    public GameObject endDayCanvas;
    public AudioSource doorBell;
    public AudioSource alarmWatch;

    public Text totalMoneyTMP;
    //public Light directionalLight;

    [Header("Settings")]
    [Description("En segundos")]
    public float dayTime = 240;
    public int dayTotal = 999;

    public int startingMinutesOffset = 17;
    public Color greenMoneyColor = new Color(18, 255, 75);
    public Color redMoneyColor = new Color(255, 73, 18);

    //public Color lightNightColor;

    [Header("Others")]
    public int daysCounter = 0;


    [Header("General Game")]
    public int money = 100; //-50 -> ESCENA DE PERDIDA












    private float timeCounter = 0;

    public static DayManager instance;

    private bool needChangeDay = false;

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
        endDayCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime;
        if (timeCounter >= dayTime)
        {
            //Fin del dia
            if (!needChangeDay) {
                needChangeDay = true;
                if (alarmWatch) alarmWatch.Play();

                if (daysCounter >= dayTotal)
                {
                    //Pantalla victoria (bueno se ha quitado al parecer)
                   
                }
                else
                {
                    //Recargar
                    endDayCanvas.SetActive(true);
                    if (money > 0)
                    {
                        totalMoneyTMP.color = greenMoneyColor;
                        totalMoneyTMP.text = "+" + money.ToString();
                    }
                    else
                    {
                        totalMoneyTMP.color = redMoneyColor;
                        totalMoneyTMP.text = money.ToString();
                    }
                    
                    //directionalLight.color = lightNightColor;

                }
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
        if (daysCounter == 1)
        {
            return daysCounter + " day";
        }
        else
        {
            return daysCounter + " days";
        }
            
    }

    public void GoNextDay()
    {
        daysCounter++;
        needChangeDay = false;
        timeCounter = 0;
        endDayCanvas.SetActive(false);
        if (doorBell)
        {
            doorBell.Play();
        }
        
    }
}
