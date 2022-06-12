using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaFade : MyMonoBehaviourSubject
{
    [SerializeField]
    private Image fadeImage = null;
    [SerializeField]
    private float fadeInSpeed = 0.01f;
    [SerializeField]
    private float fadeOutSpeed = 0.01f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float minAlpha = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float maxApha = 1.0f;

    enum FadeState
    {
        FadeIn,
        FadeOut,
        None
    };
    [ReadOnly]
    private FadeState state = FadeState.None;


    private void Update()
    {
        switch (state)
        {
            case FadeState.FadeIn:
                FadeIn();
                break;
            case FadeState.FadeOut:
                FadeOut();
                break;
        }
    }

    public void StartFadeIn()
    {
        state = FadeState.FadeIn;
        fadeImage.enabled = true;
    }

    public void StartFadeOut()
    {
        state = FadeState.FadeOut;
        fadeImage.enabled = true;
    }


    private void FadeIn()
    {
        var color = fadeImage.color;
        color.a -= fadeInSpeed;
        fadeImage.color = color;
        if (color.a <= minAlpha)
        {
            state = FadeState.None;
            fadeImage.enabled = false;
        }
    }

    private void FadeOut()
    {
        var color = fadeImage.color;
        color.a += fadeOutSpeed;
        fadeImage.color = color;
        if (color.a >= maxApha)
        {
            state = FadeState.None;
            Notify(gameObject, ObserverMessage.CLEARDIRECTION_FADEOUT_END);
        }
    }
}
