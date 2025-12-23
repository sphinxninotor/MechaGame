using System;
using UnityEngine;
using UnityEngine.Events;

public class GameSystem
{
    public static GameSystem Instance;

    public enum GAME_STATE
    {
        INTRO,
        GAMBLE,     // le joueur choisit son pari
        SPINNING,   // roulette en cours
        WIN,
        LOSE,
        GAME_OVER
    }

    private static GAME_STATE _currentGameState;
    public static GAME_STATE CurrentGameState { get => _currentGameState; set => OnChangeState(value); }
    public static event Action<GAME_STATE> OnStateChanged;
    public enum COLORS
    {
        BLACK,
        RED,
        GREEN,
    }

    private static COLORS _currentColor;
    public static COLORS CurrentColor { get => _currentColor; set => OnChangeColor(value); }
    public static event Action<COLORS> OnColorChanged;



    //permet de changer de state
    public static void OnChangeState(GAME_STATE newState)
    {
        _currentGameState = newState;
        OnStateChanged?.Invoke(newState);
    }

    public static void OnChangeColor(COLORS newState)
    {
        _currentColor = newState;
        OnColorChanged?.Invoke(newState);
    }


   
}

