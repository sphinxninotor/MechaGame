using TMPro;
using UnityEngine;
using static GameSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Nombre Parié")]
    [SerializeField] private TMP_InputField inputFieldBetNumber;
    [SerializeField] private int chosenNumber = 32;     // choix du joueur
    [SerializeField] private int lastWinningNumber = -1;


    [Header("Argent Parié")]
    [SerializeField] private TMP_InputField inputFieldAmount;
    [SerializeField] private int betAmount = 0;

    [Header("Couleur Pariée")]
    [SerializeField] private TMP_Dropdown choosenColor;


    [Header("Argent du joueur")]
    public int playerMoney = 1000;
    [SerializeField] private TextMeshProUGUI moneyDisplay;

    //Instancier le script
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

    void Start()
    {
        UpdateMoney(playerMoney);
    }

    //Modifie la couleur pariée avec des states enum
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

    //Fonction qui permet de retourner la valeur d'un input field
    private int GrabInputFieldValue(TMP_InputField field)
    {
        if(field.text == "" || !int.TryParse(field.text, out betAmount))
        {
            return 0;
        }

        return int.Parse(field.text);
    }

    //Fonction qui gère les exceptions de l'input field qui gère le nombre sur lequel on parie
    public void InputFieldExceptionBetNumber()
    {
        if (GrabInputFieldValue(inputFieldBetNumber) < 0)
        {
            inputFieldBetNumber.text = "0";
        }
        else if (GrabInputFieldValue(inputFieldBetNumber) > 36) // A MODIFIER SELON LA VALEUR MAX QU'ON AUTORISE
        {
            inputFieldBetNumber.text = "36";
        }
    }
    //Fonction qui gère les exceptions de l'input field qui gère la somme d'argent pariée
    public void InputFieldExceptionBetAmount()
    {
        if (GrabInputFieldValue(inputFieldAmount) <= 0)
        {
            inputFieldAmount.text = "1";
        }
        else if (GrabInputFieldValue(inputFieldAmount) > playerMoney)
        {
            inputFieldAmount.text = "" + playerMoney;
        }
    }

    //Modifie le text de l'argent
    public void  UpdateMoney(int money)
    {
        moneyDisplay.text = "" + money;
    }



    //permet de placer son parie sur un numéro
    public bool PlaceBet()
    {
        if (GameSystem.CurrentGameState != GAME_STATE.GAMBLE)
        {
            Debug.LogWarning("Impossible de parier maintenant");
            return false;
        }

        if (GrabInputFieldValue(inputFieldAmount) > playerMoney)
        {
            Debug.LogWarning("Pas assez d'argent !");
            return false;
        }

        chosenNumber = GrabInputFieldValue(inputFieldBetNumber);
        betAmount = GrabInputFieldValue(inputFieldAmount);
        playerMoney -= betAmount;
        UpdateMoney(playerMoney);

        Debug.Log($"BET: numero {GrabInputFieldValue(inputFieldBetNumber)}, mise de {GrabInputFieldValue(inputFieldAmount)}");
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
