using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [Header("Pages")]
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;



    private Dictionary<GameState, GameObject> GameStatePanelLookup;

    private void Awake()
    {
        GameStatePanelLookup = new Dictionary<GameState, GameObject>();
        GameStatePanelLookup.Add(GameState.GAME, InGamePanel);
        GameStatePanelLookup.Add(GameState.PAUSE, PausePanel);
        GameStatePanelLookup.Add(GameState.WIN, WinPanel);
        GameStatePanelLookup.Add(GameState.DEFEAT, LosePanel);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GM_StateChangedCallback;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GetCurrentState() == GameState.GAME)
            {
                GameManager.Instance.SwitchState(GameState.PAUSE);
                Time.timeScale = 0.0f;
                return;
            }


            if (GameManager.Instance.GetCurrentState() == GameState.PAUSE)
            {
                GameManager.Instance.SwitchState(GameState.GAME);
                Time.timeScale = 1.0f;
                return;
            }

        }
    }

    private void GM_StateChangedCallback()
    {
        UpdateWindow();
    }

    private void UpdateWindow()
    {
        GameState state = GameManager.Instance.GetCurrentState();

        if (GameStatePanelLookup.ContainsKey(state) == false)
        {
            Debug.Log($"No Window found for specific tree: {state}");
            return;
        }

        foreach (GameObject panel in GameStatePanelLookup.Values)
        {
            panel.SetActive(false);
        }

        GameStatePanelLookup[state].SetActive(true);
    }

    public void OnResumeBtn()
    {
        GameManager.Instance.SwitchState(GameState.GAME);
    }

    public void MenuBtn()
    {
        Time.timeScale = 1.0f;
        SceneTransitionManager.Instance.LoadScene("MainMenu");
    }

    public void OnRestartBtn()
    {
        Time.timeScale = 1.0f;
        SceneTransitionManager.Instance.RestartScene();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= GM_StateChangedCallback;
    }
}
