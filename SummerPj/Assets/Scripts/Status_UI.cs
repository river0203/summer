using UnityEngine;
using UnityEngine.UI;

public class Status_UI : MonoBehaviour
{
    PlayerController playercontroller;

    float _hpPercentage;
    float _staminaPercentage;
    [SerializeField] Slider Hp_Slider;
    [SerializeField] Slider Stamina_Slider;

    void Start()
    {
        playercontroller = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    void Update()
    {
        _hpPercentage = playercontroller._hp / playercontroller.maxHP;
        _staminaPercentage = playercontroller._stamina / playercontroller.maxStamina;
    }
    private void LateUpdate()
    {
        Hp_Slider.value = _hpPercentage;
        Stamina_Slider.value = _staminaPercentage;
    }
}
