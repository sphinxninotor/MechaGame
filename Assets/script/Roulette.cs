using UnityEngine;
using TMPro;
using System.Collections;

public class Roulette : MonoBehaviour
{
    public Transform visuals;         
    public TMP_Text numberText;    
    public float spinDuration = 2f;   
    public float initialSpeed = 720f; 
    public int extraFullSpins = 3; 
    public Animator animator;
   

    public int[] sliceNumbers = new int[] {
        0,32,15,19,4,21,2,25,17,34,6,27,13,36,11,30,8,23,10,5,
        24,16,33,1,20,14,31,9,22,18,29,7,28,12,35,3,26
    };

    private bool[] sliceColors = new bool[] {
        true, false, true, false, true, false, true, true, false, true, 
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false, true, false, true,
        false, true, false, true, false, true, false
    };

    public Color redColor = Color.red;
    public Color blackColor = Color.black;
    public Color greenColor = Color.green;

    private bool spinning = false;

    void Start()
    {
        GetComponents<Animator>();
        if (numberText != null)
            numberText.gameObject.SetActive(false); 
    }

    void Update()
    {

    }
    public void StartRolling()
    {
        numberText.gameObject.SetActive(false);
        animator.SetBool("spinner", true);
        if (!spinning)
            StartCoroutine(SpinAndStop());
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
            speed = Mathf.Lerp(initialSpeed, 60f, elapsed / spinDuration);
            yield return null;
        }

        int sliceCount = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers.Length : 36;
        int chosenIndex = Random.Range(0, sliceCount);
        int randomNumber = (sliceNumbers != null && sliceNumbers.Length > 0) ? sliceNumbers[chosenIndex] : Random.Range(1, 37);
        float sliceAngle = 360f / sliceCount;
        float targetSliceAngle = chosenIndex * sliceAngle; 
        float currentZ = visuals.localEulerAngles.z;
        float currentContinuous = currentZ;
        float finalAngle = currentContinuous + extraFullSpins * 360f + Mathf.DeltaAngle(currentContinuous, targetSliceAngle);

        float t = 0f;
        float smoothTime = 1.0f; 
        float startAngle = currentContinuous;
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
                numberText.color = greenColor;
            else if (chosenIndex < sliceColors.Length && sliceColors[chosenIndex])
                numberText.color = redColor;
            else
                numberText.color = blackColor;
            
            numberText.gameObject.SetActive(true); 
        }
        else Debug.LogWarning("numberText non assigné : impossible d'afficher le chiffre.");

        spinning = false;
        animator.SetBool("spinner", false);
    }
}
