using UnityEngine;
using TMPro;
using System.Collections;
using static GameSystem;

public class Roulette : MonoBehaviour
{
    COLORS stateColor;

    [SerializeField] private Transform visuals;         
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text numberTextAfter;
    [SerializeField] private TMP_Text numberTextPrior;
    [SerializeField] private float spinDuration = 2f;   
    [SerializeField] private float initialSpeed = 720f; 

    [SerializeField] private Animator animator;
   

    [SerializeField] private int[] sliceNumbers = new int[] {
        0,32,15,19,4,21,2,25,17,34,6,27,13,36,11,30,8,23,10,5,
        24,16,33,1,20,14,31,9,22,18,29,7,28,12,35,3,26
    };

    private bool[] sliceColors = new bool[] {
        true, false, true, false, true, false, true, true, false, true, 
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false
    };

    [SerializeField] private Color redColor = Color.red;
    [SerializeField] private Color blackColor = Color.black;
    [SerializeField] private Color greenColor = Color.green;

    private bool spinning = false;

    void Start()
    {
        GetComponents<Animator>();
        if (numberText != null)
        {
            numberText.gameObject.SetActive(false);
            numberTextAfter.gameObject.SetActive(false);
            numberTextPrior.gameObject.SetActive(false);
        }
    }

    public void StartRolling()
    {
        GameManager.Instance.PlaceBet();
        numberText.gameObject.SetActive(false);
        numberTextAfter.gameObject.SetActive(false);
        numberTextPrior.gameObject.SetActive(false);
        animator.SetBool("spinner", true);
        if (!spinning)
        {
            StartCoroutine(SpinAndStop());
        }
    }

    IEnumerator RollingDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartRolling();
    }

    IEnumerator SpinAndStop()
    {
        if (visuals == null)
        {
            Debug.LogError("Assigne la roue dans l'inspector.");
            yield break;
        }
        spinning = true;

        float elapsed = 0f;
        float speed = initialSpeed;
        while (elapsed < spinDuration)
        {
            visuals.Rotate(0f, 0f, speed * Time.deltaTime); 
            elapsed += Time.deltaTime;
            //speed = Mathf.Lerp(initialSpeed, 60f, elapsed / spinDuration);
            yield return null;
        }

        int sliceCount = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers.Length : 36;
        int chosenIndex = Random.Range(0, sliceCount);
        int randomNumber = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers[chosenIndex] : Random.Range(1, 37);
        float sliceAngle = 360f / sliceCount;
        float targetSliceAngle = chosenIndex * sliceAngle; 
        float currentZ = visuals.localEulerAngles.z;
        float currentContinuous = currentZ;
        float finalAngle = currentContinuous + Mathf.DeltaAngle(currentContinuous, targetSliceAngle);

        float t = 0f;
        float smoothTime = 1.0f; 
        float startAngle = currentContinuous;

        Debug.Log(startAngle + " and " + finalAngle);

        if ((startAngle > finalAngle))
        {
            float timer = 0f;
            while (timer < smoothTime)
            {
                float newZ = Mathf.LerpAngle(startAngle, startAngle + 179, timer / smoothTime);
                visuals.localEulerAngles = new Vector3(visuals.localEulerAngles.x, visuals.localEulerAngles.y, newZ);
                timer += Time.deltaTime;
                yield return null;
            }
            startAngle = visuals.localEulerAngles.z;
        }

        while (t < smoothTime)
        {
            float newZ = Mathf.LerpAngle(startAngle, finalAngle, t / smoothTime);
            visuals.localEulerAngles = new Vector3(visuals.localEulerAngles.x, visuals.localEulerAngles.y, newZ);
            t += Time.deltaTime;
            yield return null;
        }

        visuals.localEulerAngles = new Vector3(visuals.localEulerAngles.x, visuals.localEulerAngles.y, finalAngle);

        if (numberText != null)
        {
            numberText.text = randomNumber.ToString();

            if (randomNumber == 0)
            {
                numberText.color = greenColor;
                stateColor = COLORS.GREEN;
            }
            else if (chosenIndex < sliceColors.Length && sliceColors[chosenIndex])
            {
                numberText.color = redColor;
                stateColor = COLORS.RED;
            }
            else
            {
                numberText.color = blackColor;
                stateColor = COLORS.BLACK;
            }

            numberText.gameObject.SetActive(true);
        }

        if (numberTextAfter != null)
        {
            var indexAfter = chosenIndex + 1;
            if (indexAfter >= sliceNumbers.Length)
            {
                indexAfter = 0;
            }
            var numberAfter = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers[indexAfter] : Random.Range(1, 37);

            numberTextAfter.text = numberAfter.ToString();
            numberTextAfter.gameObject.SetActive(true);
        }

        if (numberTextPrior != null)
        {
            var indexPrior = chosenIndex - 1;
            if (indexPrior < 0)
            {
                indexPrior = sliceNumbers.Length - 1;
            }
            var numberPrior = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers[indexPrior] : Random.Range(1, 37);

            numberTextPrior.text = numberPrior.ToString();
            numberTextPrior.gameObject.SetActive(true);
        }

        else Debug.LogWarning("numberText non assigné : impossible d'afficher le chiffre.");

        spinning = false;
        animator.SetBool("spinner", false);

        GameManager.Instance.OnRouletteStopped(int.Parse(numberTextAfter.text), randomNumber, int.Parse(numberTextPrior.text), stateColor);
    }
}
