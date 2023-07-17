using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetMaxStamina(int maxStamina)
    {
        _slider.maxValue = maxStamina;
        _slider.value = maxStamina;
    }

    public void SetCurrentStamina(int currentStamina)
    {
        _slider.value = currentStamina;
    }
}
