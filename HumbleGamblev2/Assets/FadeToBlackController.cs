using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlackController : MonoBehaviour
{
    private float fadeToBlackTimer = 1.5f;
    private float contador = 0;
    private bool starting = true;
    private Image fadeToBlackImage;

    public bool fading = false;
    public bool fadeToBlackDoned = false;

    public static FadeToBlackController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        contador = 0;
        starting = true;
        fadeToBlackImage = GetComponentInChildren<Image>();
        fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (starting) { 
            FadeToTransparent();
        }

        if (fading) {
            FadeToBlack();
        }
    }


    public void DoFade()
    {
        if (!fading)
        {
            fading = true;
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0);
            contador = 0;
        }
        
    }

    void FadeToTransparent()
    {
        contador += Time.deltaTime;
        float alpha = Mathf.Min(contador / fadeToBlackTimer, 1.0f);
        alpha = 1.0f - alpha;

        if (contador > fadeToBlackTimer) {
            starting = false;
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 0);
        }
        else
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, alpha);
        }
    }

    void FadeToBlack()
    {
        contador += Time.deltaTime;
        float alpha = Mathf.Min(contador / fadeToBlackTimer, 1.0f);

        if (contador > fadeToBlackTimer)
        {
            fading = false;
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, 1);
            fadeToBlackDoned = true;
        }
        else
        {
            fadeToBlackImage.color = new Color(fadeToBlackImage.color.r, fadeToBlackImage.color.g, fadeToBlackImage.color.b, alpha);
        }
    }
}
