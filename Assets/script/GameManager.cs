using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using static GameSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pari")]
    [SerializeField] private int betAmount = 0;

    [SerializeField] private int chosenNumber = 32;     // choix du joueur


    [SerializeField] private int lastWinningNumber = -1;

    [SerializeField] private TMP_InputField betNumber;
    [SerializeField] private TMP_InputField betMoneyAmount;
    [SerializeField] private TMP_Dropdown choosenColor;


    [Header("Argent du joueur")]
    public int playerMoney = 1000;

    void Awake()
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintColor()
    {
        if (choosenColor.value == 0)
        {
            Debug.Log("BLACK");
        }
        else if (choosenColor.value == 1)
        {
            Debug.Log("RED");
        }
        else
        {
            Debug.Log("GREEN");
        }
    }

    void PlaceColor()
    {

    }

    //permet de placer son parie sur un numéro
    public bool PlaceBet(int number, int amount)
    {
        if (GameSystem.CurrentGameState != GAME_STATE.GAMBLE)
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

    public void OnRouletteStopped(int winningNumber)
    {
        lastWinningNumber = winningNumber;

        Debug.Log("Resultat roulette : " + winningNumber);

        if (chosenNumber == winningNumber)
        {
            Debug.Log("GAGNE");
            Win();
        }
        else
        {
            Debug.Log("Perdu");
            Lose();
        }
    }

    private void Win()
    {
        int gain = betAmount * 36;   // roulette europeenne
        playerMoney += gain;
        Debug.Log("VICTOIRE ! +" + gain);
        //GameSystem.CurrentGameState = GameSystem.GAME_STATE.WIN;
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
