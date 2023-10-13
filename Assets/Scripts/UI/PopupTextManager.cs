using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PopupTextManager : MonoBehaviour
{
    public static PopupTextManager Instance { get; private set; } 
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
    [SerializeField] List<string> badStrings = new List<string>(); 
    [SerializeField] List<string> crankyStrings = new List<string>();
    [SerializeField] List<string> nuetralStrings = new List<string>();
    [SerializeField] List<string> positiveStrings = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  
        }
        else
        {
            Destroy(gameObject);
        }
    } 
    enum textCategory
    {
        random,
        bad, 
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

    public void DisplayCandyPopupAtLocation(Vector3 worldPos, int candyValue)
    {
        if (candyPopup)
        {
            candyPopup.gameObject.SetActive(true);
            candyPopup.transform.position = worldPos;
            candyPopup.DisplayPopup(candyValue);

        }
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

        // 0,4-6 == neutral. >7 == positive, <4 cranky, 0 == bad
        if (candyValue > 6) txtCategory = textCategory.positive;
        else if (candyValue < 4) txtCategory = textCategory.cranky;
        else txtCategory = textCategory.nuetral;

        if (candyValue == 0) txtCategory = textCategory.bad;
        // based on the amount of candy given, we update our category and generate a random string from the desired list.
        int randNum = 0;

        switch (txtCategory)
        {
            case textCategory.bad:
                if (badStrings.Count > 0)
                {
                    randNum = UnityEngine.Random.Range(0, badStrings.Count);
                    popupText.text = badStrings[randNum];
                }
                break;

            case textCategory.cranky:
                if (crankyStrings.Count > 0)
                {
                    randNum = UnityEngine.Random.Range(0, crankyStrings.Count);
                    popupText.text = crankyStrings[randNum];
                }
                break;

            case textCategory.nuetral:
                if (nuetralStrings.Count > 0)
                {
                    randNum = UnityEngine.Random.Range(0, nuetralStrings.Count);
                    popupText.text = nuetralStrings[randNum];
                }
                break;

            case textCategory.positive:
                if (positiveStrings.Count > 0)
                {
                    randNum = UnityEngine.Random.Range(0, positiveStrings.Count);
                    popupText.text = positiveStrings[randNum];
                }
                break;

            case textCategory.random:
                break;

            default:
                // Handle unexpected category or empty list here
                break;
        }
    } 

}
