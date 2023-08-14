using UnityEngine;
using UnityEngine.UI;


public class MobHealthBar : MonoBehaviour
{
    public Slider _slider;
    float _timeUntillBarIsHidden = 0;

    public void SetHealth(int health)
    {
        _slider.value = health;
        _timeUntillBarIsHidden = 3;
    }

    public void SetMaXHealth(int maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
    }

    private void Update()
    {
        _timeUntillBarIsHidden -= Time.deltaTime;

        if(_slider != null)
        {
            if (_timeUntillBarIsHidden <= 0)
            {
                _timeUntillBarIsHidden = 0;
                _slider.gameObject.SetActive(false);
            }
            else
            {
                if (!_slider.gameObject.activeInHierarchy)
                {
                    _slider.gameObject.SetActive(true);
                }
            }

            if (_slider.value <= 0)
            {
                Destroy(_slider.gameObject);
            }
        }
    }
}

