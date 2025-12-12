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
        LOSE
    }

    [Header("State actuel")]
    private static GAME_STATE _currentGameState;
    public static GAME_STATE CurrentGameState { get => _currentGameState; set => OnChangeState(value); }
    public static event Action<GAME_STATE> OnStateChanged;

    [Header("Pari")]
    private int betAmount = 0;
    public int BetAmount => betAmount;

    private int chosenNumber = -1;     // choix du joueur
    public int ChosenNumber => chosenNumber;


    private int lastWinningNumber = -1;
    public int LastWinningNumber => lastWinningNumber;


    [Header("Argent du joueur")]
    private int playerMoney = 1000;
    public int PlayerMoney => playerMoney;


    //permet de changer de state
    private static void OnChangeState(GAME_STATE newState)
    {
        _currentGameState = newState;
        OnStateChanged?.Invoke(newState);
    }

    public bool PlaceBet(int number, int amount)
    {
        if (_currentGameState != GAME_STATE.GAMBLE)
        {
            Debug.LogWarning("Impossible de parier maintenant");
            return false;
        }

        if (amount > playerMoney)
        {
            Debug.LogWarning("Pas assez d'argent !");
            return false;
        }

        chosenNumber = number;
        betAmount = amount;
        playerMoney -= amount;

        Debug.Log($"BET: numero {number}, mise de {amount}");
        return true;
    }

    public void StartSpin(Func<int> rouletteStartFunction)
    {
        // Appelé par roulette / bouton spin
        if (_currentGameState != GAME_STATE.GAMBLE)
            return;

        OnChangeState(GAME_STATE.SPINNING);

        // Lancer la roulette et recuperer le resultat via callback
        int result = rouletteStartFunction.Invoke();
        OnRouletteStopped(result);
    }

    public void OnRouletteStopped(int winningNumber)
    {
        lastWinningNumber = winningNumber;

        Debug.Log("Resultat roulette : " + winningNumber);

        if (chosenNumber == winningNumber)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void Win()
    {
        int gain = betAmount * 36;   // roulette europeenne
        playerMoney += gain;
        Debug.Log("VICTOIRE ! +" + gain);
        OnChangeState(GAME_STATE.WIN);
    }

    private void Lose()
    {
        Debug.Log("Defaite… - " + betAmount);
        OnChangeState(GAME_STATE.LOSE);
    }

    public void Replay()
    {
        chosenNumber = -1;
        betAmount = 0;
        OnChangeState(GAME_STATE.GAMBLE);
    }
}

