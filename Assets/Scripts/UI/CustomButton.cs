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

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = imageHighlightColor;
        textMesh.color = hoverTextColor;

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = imageNormalColor;
        textMesh.color = normalTextColor;

        if (hoverSound)
            AudioManager.Instance.PlayAudioSFX(clickSound);
    }
}
