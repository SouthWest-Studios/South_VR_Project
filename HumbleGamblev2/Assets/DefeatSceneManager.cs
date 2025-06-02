using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeToBlackController.instance.fadeToBlackDoned)
        {
            SceneManager.LoadScene("MartiScene");
        }
    }


    public void RetryGame()
    {
        if (!FadeToBlackController.instance.fading && !FadeToBlackController.instance.fadeToBlackDoned)
        {
            FadeToBlackController.instance.DoFade();
        }
    }
}
