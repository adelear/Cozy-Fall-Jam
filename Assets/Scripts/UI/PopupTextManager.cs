using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PopupTextManager : MonoBehaviour
{
    //this script is used to display world text popups throughout the map
    //we meed to set its position and text based on - where the player interacted with house - how much candy was given

    //values 
    [SerializeField] float popupLifetime = 1.0f;
    
    private float popupTextDefOpacity = 1.0f;
    private float popupBckgDefOpacity = 1.0f;
    private float fadeTime = 1.0f;
    
    //UI stuffs
    [SerializeField] Image popupBackground;
    [SerializeField] TextMeshProUGUI popupText;

    public bool test = false;
    private void Start()
    {
        popupBckgDefOpacity = popupBackground.color.a;
        popupTextDefOpacity = popupText.color.a;

        fadeTime = popupLifetime / 2;

    }
    private void Update()
    {
        if(test)
        {
            StartCoroutine(DisplayPopup());
            test = false;
        }

    }

    public void DisplayPopupAtLocation(Vector3 worldPos, string text)
    {
        transform.position = worldPos;
        popupText.text = text;

        StartCoroutine(DisplayPopup());
    }
    IEnumerator DisplayPopup()
    {
        SetPopupBackgroundOpacity(popupBckgDefOpacity);
        SetPopupTextOpacity(popupTextDefOpacity);


        yield return new WaitForSeconds(fadeTime);

        float tempDur = 0;

        while (tempDur <= fadeTime)
        {
            float opacPcnt = tempDur / fadeTime;

            SetPopupBackgroundOpacity(popupBckgDefOpacity - (popupBckgDefOpacity * opacPcnt));
            SetPopupTextOpacity(popupTextDefOpacity - (popupTextDefOpacity * opacPcnt));

            tempDur += Time.deltaTime;

            yield return null;
        }
    }

    public void SetText(string text)
    {
        popupText.text = text;
    }


    private void SetPopupBackgroundOpacity(float opacity)
    {
        popupBackground.color = new Color(popupBackground.color.r, popupBackground.color.g, popupBackground.color.b, opacity);
    }
    private void SetPopupTextOpacity(float opacity)
    {
        popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, opacity);

    }
}
