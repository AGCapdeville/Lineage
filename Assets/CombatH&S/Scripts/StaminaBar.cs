using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mirror;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    private float maxStamina = 100f;
    private float currentStamina;

    private WaitForSeconds regenTik = new WaitForSeconds(0.1f);
    private WaitForSeconds regenDelay = new WaitForSeconds(2);
    private Coroutine regen;

    public static StaminaBar instance;

    public void Awake()
    {
        // Need to change how we handle accesssing the players stamina and other stats. ( Possibly singletons) 
        // For now this will work for a single scene. 
        instance = this;
    }

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void UseStamina(float amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if (regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
        }
        else
        {
            Debug.Log("Not enough stamina!");
        }
    }

    public bool IsStaminaEmpty()
    {
        if (currentStamina < 10f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private IEnumerator RegenStamina()
    {
        yield return regenDelay;

        while (currentStamina < maxStamina)
        {
            currentStamina += maxStamina / 100f;
            staminaBar.value = currentStamina;
            yield return regenTik;
        }
        regen = null;
    }
}
