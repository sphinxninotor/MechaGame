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
    [SerializeField] private TextMeshProUGUI moneyDisplay;

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
            OnChangeColor(COLORS.BLACK);
        }
        else if (choosenColor.value == 1)
        {
            Debug.Log("RED");
            OnChangeColor(COLORS.RED);
        }
        else
        {
            Debug.Log("GREEN");
            OnChangeColor(COLORS.GREEN);
        }
    }

    public int GrabInputFieldValue(TMP_InputField field)
    {
        if(field.text == null || !int.TryParse(field.text, out betAmount))
        {
            return 0;
        }

        return int.Parse(field.text);
    }


    //permet de placer son parie sur un numéro
    public bool PlaceBet()
    {
        if (GameSystem.CurrentGameState != GAME_STATE.GAMBLE)
        {
            Debug.LogWarning("Impossible de parier maintenant");
            return false;
        }

        if (GrabInputFieldValue(betMoneyAmount) > playerMoney)
        {
            Debug.LogWarning("Pas assez d'argent !");
            return false;
        }

        chosenNumber = GrabInputFieldValue(betNumber);
        betAmount = GrabInputFieldValue(betMoneyAmount);
        playerMoney -= GrabInputFieldValue(betMoneyAmount);

        Debug.Log($"BET: numero {GrabInputFieldValue(betNumber)}, mise de {GrabInputFieldValue(betMoneyAmount)}");
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
