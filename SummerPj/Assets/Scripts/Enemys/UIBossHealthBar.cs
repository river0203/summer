using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHealthBar : MonoBehaviour
{
    public Text _bossName;
    Slider _slier;

    private void Awake()
    {
        _slier = GetComponentInChildren<Slider>();
        _bossName = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        SetHealthBarToInactive();
    }

    public void SetBossName(string _name)
    {
        _bossName.text = _name;
    }

    public void SetUIHealthBarToActive()
    {
        _slier.gameObject.SetActive(true);  
    }

    public void SetHealthBarToInactive()
    {
        _slier.gameObject.SetActive(false);
    }

    public void SetBossMaxHealth(int _maxHealth)
    {
        _slier.maxValue = _maxHealth;   
        _slier.value = _maxHealth;
    }

    public void SetBossCurrentHealth(int _currentHealth)
    {
        _slier.value = _currentHealth;
    }
}
