using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Jumping,
    InMiniGame,
    InInventory,
    InEscapeMenu,
    RodCharging
}

public class PlayerStateHandler : MonoBehaviour
{
    public static PlayerStateHandler Instance { get; private set; }
    public PlayerState CurrentState { get; private set; } = PlayerState.Idle;

    // Event to notify state changes
    public event Action<PlayerState> OnStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Method to change the state
    public void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        Debug.Log("State changed to: " + CurrentState);

        // Notify listeners
        OnStateChanged?.Invoke(CurrentState);
    }
}
