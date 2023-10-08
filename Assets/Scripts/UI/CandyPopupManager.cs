using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CandyPopupManager : MonoBehaviour
{
    
    [SerializeField] float popupLifetime = 1.0f;
    [SerializeField] float yOffset = 5.0f;
   
    [SerializeField] Transform playerTForm;
    [SerializeField] Image candyImg;
    [SerializeField] TextMeshProUGUI popupText;
    
    private float fadeTime = 1.0f;

    private void Start()
    {
        playerTForm = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void DisplayPopup(int candyValue)
    {
        StopAllCoroutines();

        if (!playerTForm) playerTForm = GameObject.FindGameObjectWithTag("Player").transform;

        transform.position = new Vector3(playerTForm.position.x, playerTForm.position.y + yOffset, playerTForm.position.z);
        Debug.Log("Generate candy mssg");

        //popupText.text = text;

        DecideString(candyValue);

        StartCoroutine(DisplayPopup());
    }

    IEnumerator DisplayPopup()
    {
        SetOpacity(1);
        SetOpacity(1);


        yield return new WaitForSeconds(fadeTime);

        float tempDur = 0;

        while (tempDur <= fadeTime)
        {
            float opacPcnt = tempDur / fadeTime;

            SetOpacity(1 - (1 * opacPcnt));
            SetOpacity(1- (1 * opacPcnt));

            tempDur += Time.deltaTime;

            yield return null;
        }
    }

    private void SetOpacity(float opacity)
    {
         
        Color color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, opacity);

        popupText.color = color;
        candyImg.color = color;

    }

    private void DecideString(int candyvalue)
    {
        popupText.text = "x" + candyvalue;
    }

}
