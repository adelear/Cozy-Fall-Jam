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
    [SerializeField] CandyPopupManager candyPopup;
    

    //Strings and categories
    [SerializeField] List<string> crankyStrings = new List<string>();
    [SerializeField] List<string> nuetralStrings = new List<string>();
    [SerializeField] List<string> positiveStrings = new List<string>();

    enum textCategory
    {
        random,
        cranky,
        nuetral,
        positive
    }

    textCategory txtCategory = textCategory.random;

    public bool test = false;

    private void Start()
    {
        popupBckgDefOpacity = popupBackground.color.a;
        popupTextDefOpacity = popupText.color.a;

        fadeTime = popupLifetime / 2;

        candyPopup = Instantiate(candyPopup.gameObject, transform.position, Quaternion.identity).GetComponent<CandyPopupManager>();


        SetPopupBackgroundOpacity(0);
        SetPopupTextOpacity(0);

    }
    private void Update()
    {
        if(test)
        {
            StartCoroutine(DisplayPopup());
            test = false;
        }

    }

    public void DisplayPopupAtLocation(Vector3 worldPos, int candyValue)
    {
        StopAllCoroutines();

        transform.position = worldPos;
        Debug.Log("Generate candy mssg");

        //popupText.text = text;

        DecideString(candyValue);

        if (candyPopup)
        {
            candyPopup.gameObject.SetActive(true);
            candyPopup.DisplayPopup(candyValue);
        }
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

    private void DecideString(int candyValue)
    {
        txtCategory = textCategory.random;

        //4-6 == nuetral. >7 == positive, <4 cranky
        if (candyValue > 6) txtCategory = textCategory.positive;
        else if (candyValue < 4) txtCategory = textCategory.cranky;
        else txtCategory = textCategory.nuetral;

        //based on the amount of candy given , we update our category and generate a random string from the desired list.
        int randNum = 0;

        switch (txtCategory)
        {
            case textCategory.cranky:
                
                randNum = UnityEngine.Random.Range(0, crankyStrings.Count);
                popupText.text = crankyStrings[randNum];
                break;

            case textCategory.nuetral:

                randNum = UnityEngine.Random.Range(0, nuetralStrings.Count);
                popupText.text = nuetralStrings[randNum];
                break;
            
            case textCategory.positive:

                randNum = UnityEngine.Random.Range(0, positiveStrings.Count);
                popupText.text = positiveStrings[randNum];
                break;
            
            case textCategory.random:
                break;
        }

    }
}
