using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameStates(GameState.turnIndex);
    }

    public void UpdateGameStates(GameState newState)
    {
        State = newState;

        switch (State)
        {
            case GameState.turnIndex:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    turnIndex
}