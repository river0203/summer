using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider _slider;

    public void Init(int maxHealth)
    {
        if (_slider == null)
            _slider = GetComponent<Slider>();

        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        _slider.value = currentHealth;
    }
}
