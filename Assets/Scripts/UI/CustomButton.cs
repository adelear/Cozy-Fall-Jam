using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    private TextMeshProUGUI textMesh;

    [SerializeField] private Color imageNormalColor;
    [SerializeField] private Color imageHighlightColor;

    [SerializeField] private Color normalColor = new Color(244f, 248f, 232f);
    [SerializeField] private Color hoverColor = new Color(123f, 28f, 20f);

    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = imageHighlightColor;
        textMesh.color = hoverColor;

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = imageNormalColor;
        textMesh.color = normalColor;

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(clickSound);
    }
}
