using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameState currentState;
    public event Action OnGameStateChanged;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (Instance != this)
            Destroy(gameObject);
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void SwitchState(GameState newState)
    {
        //if (newState == currentState)
        //{
        //    Debug.Log("Trying to change game state to its current state");
        //    return;
        //}

        Debug.Log($"New state has been set to: {newState}");

        currentState = newState;
        OnGameStateChanged?.Invoke();
    }
}

public enum GameState
{
    GAME,
    PAUSE,
    DEFEAT,
    WIN
}
