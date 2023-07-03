using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;

    private const float defaultFadeDuration = 2;

    private FadeStatus fadeStatus = FadeStatus.Idle;
    public FadeStatus FadeStatus
    {
        get { return fadeStatus; }
    }

    public void StartFadeOut(float fadeDuration = defaultFadeDuration)
    {
        fadeStatus = FadeStatus.Fading;
        StartCoroutine(FadeOut(fadeDuration));
    }

    public void StartFadeIn(float fadeDuration = defaultFadeDuration)
    {
        fadeStatus = FadeStatus.Fading;
        StartCoroutine(FadeIn(fadeDuration));
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeImageAlpha = Mathf.Clamp(elapsedTime / fadeDuration, 0, 1);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageAlpha);
            yield return null;
        }

        fadeStatus = FadeStatus.Finished;
    }

    private IEnumerator FadeIn(float fadeDuration)
    {
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fadeImageAlpha = Mathf.Clamp(1.0f - (elapsedTime / fadeDuration), 0, 1);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImageAlpha);
            yield return null;
        }

        fadeStatus = FadeStatus.Finished;
    }
}
