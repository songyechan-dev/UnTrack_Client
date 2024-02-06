using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LoadingImageTween : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 0.8f;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        canvasGroup.alpha = 1;
        Tween tween = canvasGroup.DOFade(0.0f, fadeSpeed);
        yield return tween.WaitForCompletion();

        canvasGroup.alpha = 0;
        tween = canvasGroup.DOFade(1.0f, fadeSpeed);
        yield return tween.WaitForCompletion();

        StartCoroutine(Fade());
    }
}
