using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider _slider;

    public void Init(int maxStamina)
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        _slider.maxValue = maxStamina;
        _slider.value = maxStamina;
    }

    public void SetCurrentStamina(int currentStamina)
    {
        _slider.value = currentStamina;
    }
}
