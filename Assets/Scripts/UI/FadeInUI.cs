using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CanvasGroup backgroundImg;
    [SerializeField] private CanvasGroup textGroup;

    [Header("Transition Timings")]
    [SerializeField] private float initialDelay = 0.3f;
    [SerializeField] private float timeToFadeBlack = 0.2f;
    [SerializeField] private float timeToDisplayText = 0.3f;
    [SerializeField] private AnimationCurve displayCurve;

    private void OnEnable()
    {
        Cursor.visible = true;
        StartCoroutine(HandleDisplay());
    }

    private IEnumerator HandleDisplay()
    {

        backgroundImg.alpha = 0;
        textGroup.alpha = 0;

        yield return new WaitForSecondsRealtime(initialDelay);

        float timeElapsed = 0.0f;

        while (timeElapsed <= timeToFadeBlack)
        {
            timeElapsed += Time.unscaledDeltaTime;
            backgroundImg.alpha = Mathf.Lerp(0, 1, timeElapsed / timeToFadeBlack);
            yield return null;
        }

        yield return null;

        timeElapsed = 0.0f;
        while (timeElapsed <= timeToDisplayText)
        {
            timeElapsed += Time.unscaledDeltaTime;

            float alpha = displayCurve.Evaluate(timeElapsed / timeToDisplayText);
            textGroup.alpha = Mathf.Lerp(0, 1, alpha);
            yield return null;
        }
    }
}
