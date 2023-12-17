using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float timeToFade;
    [SerializeField] float afterFadeDelay = 3f;

    public bool fadeIn = false;
    public bool fadeout = false;
    int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        StartCoroutine(FadeInAndOut());
    }

    private void Update()
    {
        if (fadeIn == true)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;
                if (canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeout == true)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;
                if (canvasGroup.alpha == 0)
                {
                    fadeout = false;
                }
            }
        }
    }

    IEnumerator FadeInAndOut()
    {
        FadeOut();
        yield return new WaitForSeconds(afterFadeDelay);
        FadeIn();
        yield return new WaitForSeconds(afterFadeDelay);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeOut()
    {
        fadeout = true;
    }

}
