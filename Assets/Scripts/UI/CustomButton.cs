using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMesh;

    [SerializeField] private Color imageNormalColor;
    [SerializeField] private Color imageHighlightColor;

    [SerializeField] private Color normalTextColor = new Color(244f, 248f, 232f);
    [SerializeField] private Color hoverTextColor = new Color(123f, 28f, 20f);

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private float transitionTime = 0.1f;
    [SerializeField] private AnimationCurve transitionCurve;

    private Coroutine transitionRoutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //image.color = imageHighlightColor;
        //textMesh.color = hoverTextColor;

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(ButtonTransition(true));

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(hoverSound);
    }

    private IEnumerator ButtonTransition(bool hover)
    {
        float start = image.fillAmount;
        float end = hover ? 1 : 0;

        float timeElapsed = 0.0f;
        while (timeElapsed <= transitionTime)
        {
            timeElapsed += Time.unscaledDeltaTime;
            float t = transitionCurve.Evaluate(timeElapsed / transitionTime);
            image.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //image.color = imageNormalColor;
        //textMesh.color = normalTextColor;

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(ButtonTransition(false));

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(clickSound);
    }
}
