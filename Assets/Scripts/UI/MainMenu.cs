using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    [Header("Strings")]

    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private Button creditsBtn;
    [SerializeField] private Button settingsBackBtn;
    [SerializeField] private Button creditsBackBtn;

    [Header("Pages")]
    [SerializeField] private GameObject mainPage;
    [SerializeField] private GameObject settingsPage;
    [SerializeField] private GameObject creditsPage;

    //[SerializeField] MainMenuAnimation menuAnim;

    private void Start()
    {
        playBtn.onClick.AddListener(OnPlayBtnClicked);
        settingsBtn.onClick.AddListener(OnSettingsBtnClicked);
        creditsBtn.onClick.AddListener(OnCreditsBtnClicked);

        settingsBackBtn.onClick.AddListener(BackBtnClicked);
        creditsBackBtn.onClick.AddListener(BackBtnClicked);
    }

    public void OpenCloseMenu(GameObject openMenu, GameObject closeMenu)
    {
        closeMenu.SetActive(false);
        openMenu.SetActive(true);

        // prolly have transitions later on
    }

    private void OnCreditsBtnClicked()
    {
        OpenCloseMenu(creditsPage, mainPage);
    }

    private void OnSettingsBtnClicked()
    {
        OpenCloseMenu(settingsPage, mainPage);
    }

    private void BackBtnClicked()
    {
        settingsPage.SetActive(false);
        creditsPage.SetActive(false);
        mainPage.SetActive(true);
    }

    private void OnPlayBtnClicked()
    {
        //menuAnim.PlayAnimation();
        StartCoroutine(WaitOneSecond());
    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("WhiteBoxLevel");  
    }
}
